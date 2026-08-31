using System.Text.Json;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.Cms.KtuSaModule.Models.Backup;
using OrchardCore.Data;
using OrchardCore.Modules;

namespace OrchardCore.Cms.KtuSaModule.Services;

public sealed class DatabaseBackupService(
    IDbConnectionAccessor dbConnectionAccessor,
    IClock clock,
    ILogger<DatabaseBackupService> logger) : IDatabaseBackupService
{
    private const int MaxSnapshots = 5;

    private static readonly byte[] SqliteMagic = "SQLite format 3\0"u8.ToArray();

    private static readonly string[] RequiredTables = ["Document", "ContentItemIndex"];

    public DatabaseBackupStatus GetStatus()
    {
        if (!TryGetDatabasePath(out var databasePath, out var reason))
            return new DatabaseBackupStatus { IsSupported = false, UnsupportedReason = reason };

        var file = new FileInfo(databasePath);
        var directory = Path.GetDirectoryName(databasePath)!;

        return new DatabaseBackupStatus
        {
            IsSupported = true,
            DatabaseSizeBytes = file.Exists ? file.Length : 0,
            DatabaseModifiedUtc = file.Exists ? file.LastWriteTimeUtc : null,
            PendingRestore = ReadPendingRestore(directory),
            Snapshots = ReadSnapshots(directory)
        };
    }

    public string BuildBackupFileName()
    {
        return $"ktusa-cms-backup-{clock.UtcNow:yyyyMMdd-HHmmss}.db";
    }

    public async Task<Stream> CreateBackupAsync(CancellationToken cancellationToken = default)
    {
        if (!TryGetDatabasePath(out var databasePath, out var reason))
            throw new InvalidOperationException(reason);

        var tempPath = Path.Combine(Path.GetTempPath(), $"ktusa-backup-{Guid.NewGuid():N}.db");

        try
        {
            await using (var source = new SqliteConnection(BuildConnectionString(databasePath)))
            await using (var destination = new SqliteConnection(BuildConnectionString(tempPath)))
            {
                await source.OpenAsync(cancellationToken);
                await destination.OpenAsync(cancellationToken);

                // SQLite's online backup API: consistent even while the site is being edited.
                source.BackupDatabase(destination);
            }

            // DeleteOnClose: the file disappears once the response finishes streaming.
            return new FileStream(
                tempPath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                bufferSize: 64 * 1024,
                FileOptions.DeleteOnClose | FileOptions.Asynchronous);
        }
        catch
        {
            TryDelete(tempPath);
            throw;
        }
    }

    public async Task<BackupOperationResult> StageRestoreAsync(
        Stream content,
        string fileName,
        string userName,
        CancellationToken cancellationToken = default)
    {
        if (!TryGetDatabasePath(out var databasePath, out var reason))
            return BackupOperationResult.Fail(reason);

        var directory = Path.GetDirectoryName(databasePath)!;
        var workspace = BackupWorkspace.GetDirectory(directory);
        Directory.CreateDirectory(workspace);

        var incomingPath = Path.Combine(workspace, $"incoming-{Guid.NewGuid():N}.db");

        try
        {
            await using (var incoming = new FileStream(
                             incomingPath,
                             FileMode.CreateNew,
                             FileAccess.Write,
                             FileShare.None,
                             bufferSize: 64 * 1024,
                             useAsync: true))
            {
                await content.CopyToAsync(incoming, cancellationToken);
            }

            var validation = ValidateSqliteFile(incomingPath);
            if (!validation.Succeeded)
            {
                TryDelete(incomingPath);
                return validation;
            }

            var pendingPath = BackupWorkspace.GetPendingDatabasePath(directory);
            TryDelete(pendingPath);
            File.Move(incomingPath, pendingPath);

            var metadata = new PendingRestore
            {
                FileName = Path.GetFileName(fileName),
                SizeBytes = new FileInfo(pendingPath).Length,
                UploadedUtc = clock.UtcNow,
                UploadedBy = userName
            };

            await File.WriteAllTextAsync(
                BackupWorkspace.GetPendingMetadataPath(directory),
                JsonSerializer.Serialize(metadata),
                cancellationToken);

            logger.LogWarning(
                "Database restore staged by {User} from {FileName} ({Bytes} bytes). Applies on next restart.",
                userName,
                metadata.FileName,
                metadata.SizeBytes);

            return BackupOperationResult.Ok(
                "Backup uploaded and verified. It will replace the live database the next time the CMS restarts.");
        }
        catch (Exception exception)
        {
            TryDelete(incomingPath);
            logger.LogError(exception, "Staging a database restore failed.");

            return BackupOperationResult.Fail($"Upload failed: {exception.Message}");
        }
    }

    public BackupOperationResult DiscardPendingRestore()
    {
        if (!TryGetDatabasePath(out var databasePath, out var reason))
            return BackupOperationResult.Fail(reason);

        var directory = Path.GetDirectoryName(databasePath)!;
        var pendingPath = BackupWorkspace.GetPendingDatabasePath(directory);

        if (!File.Exists(pendingPath)) return BackupOperationResult.Fail("There is no pending restore to discard.");

        TryDelete(pendingPath);
        TryDelete(BackupWorkspace.GetPendingMetadataPath(directory));

        logger.LogWarning("Pending database restore discarded.");

        return BackupOperationResult.Ok("Pending restore discarded. The live database is untouched.");
    }

    public Stream? OpenSnapshot(string id)
    {
        if (!BackupWorkspace.IsSnapshotId(id)) return null;
        if (!TryGetDatabasePath(out var databasePath, out _)) return null;

        var directory = Path.GetDirectoryName(databasePath)!;
        var path = Path.Combine(BackupWorkspace.GetSnapshotsDirectory(directory), id);

        if (!File.Exists(path)) return null;

        return new FileStream(
            path,
            FileMode.Open,
            FileAccess.Read,
            FileShare.Read,
            bufferSize: 64 * 1024,
            FileOptions.Asynchronous);
    }

    // Pooling is off deliberately. Microsoft.Data.Sqlite keeps a pooled connection's
    // file handle open after Dispose, which leaves the file locked: the temp backup
    // could not be streamed back, and a staged upload could not be moved into place.
    private static string BuildConnectionString(string path)
    {
        return new SqliteConnectionStringBuilder { DataSource = path, Pooling = false }.ToString();
    }

    private BackupOperationResult ValidateSqliteFile(string path)
    {
        var file = new FileInfo(path);
        if (!file.Exists || file.Length == 0) return BackupOperationResult.Fail("The uploaded file is empty.");

        if (file.Length < 512)
            return BackupOperationResult.Fail("The uploaded file is too small to be a SQLite database.");

        var header = new byte[SqliteMagic.Length];
        using (var stream = File.OpenRead(path))
        {
            if (stream.ReadAtLeast(header, header.Length, throwOnEndOfStream: false) < header.Length
                || !header.AsSpan().SequenceEqual(SqliteMagic))
                return BackupOperationResult.Fail(
                    "That is not a SQLite database file. Upload a .db file produced by the Download backup button.");
        }

        try
        {
            var connectionString = new SqliteConnectionStringBuilder
            {
                DataSource = path,
                Mode = SqliteOpenMode.ReadOnly,
                Pooling = false
            }.ToString();

            using var connection = new SqliteConnection(connectionString);
            connection.Open();

            using (var check = connection.CreateCommand())
            {
                check.CommandText = "PRAGMA quick_check;";
                var result = check.ExecuteScalar() as string;
                if (!string.Equals(result, "ok", StringComparison.OrdinalIgnoreCase))
                    return BackupOperationResult.Fail($"The database failed its integrity check: {result}");
            }

            foreach (var table in RequiredTables)
            {
                using var command = connection.CreateCommand();
                command.CommandText =
                    "SELECT COUNT(*) FROM sqlite_master WHERE type = 'table' AND name = $name;";
                command.Parameters.AddWithValue("$name", table);

                if (Convert.ToInt64(command.ExecuteScalar()) == 0)
                    return BackupOperationResult.Fail(
                        $"This SQLite file has no '{table}' table, so it is not a KTU SA CMS database.");
            }
        }
        catch (SqliteException exception)
        {
            return BackupOperationResult.Fail($"The file could not be opened as a database: {exception.Message}");
        }

        return BackupOperationResult.Ok("Verified.");
    }

    private bool TryGetDatabasePath(out string path, out string reason)
    {
        path = string.Empty;
        reason = string.Empty;

        try
        {
            using var connection = dbConnectionAccessor.CreateConnection();

            if (connection is not SqliteConnection)
            {
                reason =
                    "Database backups are only available when the tenant runs on SQLite. "
                    + $"This tenant uses {connection.GetType().Name}.";

                return false;
            }

            var dataSource = new SqliteConnectionStringBuilder(connection.ConnectionString).DataSource;

            if (string.IsNullOrWhiteSpace(dataSource))
            {
                reason = "The SQLite connection string does not point at a file.";

                return false;
            }

            path = Path.GetFullPath(dataSource);

            return true;
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Could not resolve the tenant database path.");
            reason = $"Could not resolve the database path: {exception.Message}";

            return false;
        }
    }

    private PendingRestore? ReadPendingRestore(string databaseDirectory)
    {
        var pendingPath = BackupWorkspace.GetPendingDatabasePath(databaseDirectory);
        if (!File.Exists(pendingPath)) return null;

        var metadataPath = BackupWorkspace.GetPendingMetadataPath(databaseDirectory);
        var file = new FileInfo(pendingPath);

        if (File.Exists(metadataPath))
            try
            {
                var metadata = JsonSerializer.Deserialize<PendingRestore>(File.ReadAllText(metadataPath));
                if (metadata is not null) return metadata;
            }
            catch (JsonException exception)
            {
                logger.LogWarning(exception, "Pending restore metadata could not be read.");
            }

        return new PendingRestore
        {
            FileName = BackupWorkspace.PendingDatabaseFileName,
            SizeBytes = file.Length,
            UploadedUtc = file.LastWriteTimeUtc,
            UploadedBy = "unknown"
        };
    }

    private IReadOnlyList<RestoreSnapshot> ReadSnapshots(string databaseDirectory)
    {
        var snapshotsDirectory = BackupWorkspace.GetSnapshotsDirectory(databaseDirectory);
        if (!Directory.Exists(snapshotsDirectory)) return [];

        try
        {
            return
            [
                .. new DirectoryInfo(snapshotsDirectory)
                    .EnumerateFiles($"{BackupWorkspace.SnapshotPrefix}*{BackupWorkspace.SnapshotExtension}")
                    .OrderByDescending(f => f.LastWriteTimeUtc)
                    .Take(MaxSnapshots)
                    .Select(f => new RestoreSnapshot
                    {
                        Id = f.Name,
                        SizeBytes = f.Length,
                        CreatedUtc = f.LastWriteTimeUtc
                    })
            ];
        }
        catch (IOException exception)
        {
            logger.LogWarning(exception, "Could not list restore snapshots.");

            return [];
        }
    }

    private void TryDelete(string path)
    {
        try
        {
            if (File.Exists(path)) File.Delete(path);
        }
        catch (IOException exception)
        {
            logger.LogWarning(exception, "Could not delete {Path}.", path);
        }
    }
}
