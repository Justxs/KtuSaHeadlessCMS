namespace OrchardCore.Cms.KtuSaModule.Services;

/// <summary>
/// Layout of the backup working directory that sits next to the tenant database.
/// Shared by the running application and by the startup restore applier, which
/// runs before Orchard boots and therefore cannot use dependency injection.
/// </summary>
public static class BackupWorkspace
{
    public const string DatabaseFileName = "yessql.db";

    public const string DirectoryName = "backup";

    public const string PendingDatabaseFileName = "pending.db";

    public const string PendingMetadataFileName = "pending.json";

    public const string SnapshotsDirectoryName = "snapshots";

    public const string RestoreLogFileName = "restore.log";

    public const string SnapshotPrefix = "pre-restore-";

    public const string SnapshotExtension = ".db";

    /// <summary>SQLite sidecars that must be removed when the database file is replaced.</summary>
    public static readonly string[] DatabaseSidecarSuffixes = ["-journal", "-wal", "-shm"];

    public static string GetDirectory(string databaseDirectory)
    {
        return Path.Combine(databaseDirectory, DirectoryName);
    }

    public static string GetPendingDatabasePath(string databaseDirectory)
    {
        return Path.Combine(GetDirectory(databaseDirectory), PendingDatabaseFileName);
    }

    public static string GetPendingMetadataPath(string databaseDirectory)
    {
        return Path.Combine(GetDirectory(databaseDirectory), PendingMetadataFileName);
    }

    public static string GetSnapshotsDirectory(string databaseDirectory)
    {
        return Path.Combine(GetDirectory(databaseDirectory), SnapshotsDirectoryName);
    }

    public static string GetRestoreLogPath(string databaseDirectory)
    {
        return Path.Combine(GetDirectory(databaseDirectory), RestoreLogFileName);
    }

    public static bool IsSnapshotId(string id)
    {
        return !string.IsNullOrWhiteSpace(id)
               && id.StartsWith(SnapshotPrefix, StringComparison.Ordinal)
               && id.EndsWith(SnapshotExtension, StringComparison.Ordinal)
               && id.IndexOfAny(Path.GetInvalidFileNameChars()) < 0
               && !id.Contains("..", StringComparison.Ordinal);
    }
}
