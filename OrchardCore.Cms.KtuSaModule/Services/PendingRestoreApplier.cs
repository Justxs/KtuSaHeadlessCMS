using System.Globalization;

namespace OrchardCore.Cms.KtuSaModule.Services;

/// <summary>
/// Applies a staged database restore before the application starts.
/// <para>
/// A SQLite file cannot be replaced underneath a running Orchard tenant, so the admin
/// upload only stages the file. The swap happens here, at the one moment nothing holds
/// the database open: before the host is built.
/// </para>
/// </summary>
public static class PendingRestoreApplier
{
    private const int MaxSnapshots = 5;

    /// <param name="appDataPath">Absolute path to the App_Data directory.</param>
    /// <returns>The number of tenants whose database was replaced.</returns>
    public static int Apply(string appDataPath)
    {
        var sitesPath = Path.Combine(appDataPath, "Sites");
        if (!Directory.Exists(sitesPath)) return 0;

        var applied = 0;

        foreach (var tenantDirectory in Directory.EnumerateDirectories(sitesPath))
            try
            {
                if (ApplyForTenant(tenantDirectory)) applied++;
            }
            catch (Exception exception)
            {
                // Never prevent the site from starting because a restore failed.
                Report(tenantDirectory, $"RESTORE FAILED: {exception}");
            }

        return applied;
    }

    private static bool ApplyForTenant(string tenantDirectory)
    {
        var pendingPath = BackupWorkspace.GetPendingDatabasePath(tenantDirectory);
        if (!File.Exists(pendingPath)) return false;

        var databasePath = Path.Combine(tenantDirectory, BackupWorkspace.DatabaseFileName);
        var tenant = Path.GetFileName(tenantDirectory);

        Report(tenantDirectory, $"Applying staged database restore for tenant '{tenant}'.");

        if (File.Exists(databasePath))
        {
            var snapshotsDirectory = BackupWorkspace.GetSnapshotsDirectory(tenantDirectory);
            Directory.CreateDirectory(snapshotsDirectory);

            var stamp = DateTime.UtcNow.ToString("yyyyMMdd-HHmmss", CultureInfo.InvariantCulture);
            var snapshotPath = Path.Combine(
                snapshotsDirectory,
                $"{BackupWorkspace.SnapshotPrefix}{stamp}{BackupWorkspace.SnapshotExtension}");

            File.Move(databasePath, snapshotPath, true);
            Report(tenantDirectory, $"Previous database kept as {Path.GetFileName(snapshotPath)}.");

            PruneSnapshots(snapshotsDirectory, tenantDirectory);
        }

        // A leftover journal belongs to the database we just moved away. Replaying it
        // over the restored file would corrupt it.
        foreach (var suffix in BackupWorkspace.DatabaseSidecarSuffixes)
        {
            var sidecar = databasePath + suffix;
            if (!File.Exists(sidecar)) continue;

            File.Delete(sidecar);
            Report(tenantDirectory, $"Removed stale sidecar {Path.GetFileName(sidecar)}.");
        }

        File.Move(pendingPath, databasePath, true);

        var metadataPath = BackupWorkspace.GetPendingMetadataPath(tenantDirectory);
        if (File.Exists(metadataPath)) File.Delete(metadataPath);

        Report(tenantDirectory, $"Restore complete for tenant '{tenant}'.");

        return true;
    }

    private static void PruneSnapshots(string snapshotsDirectory, string tenantDirectory)
    {
        var stale = new DirectoryInfo(snapshotsDirectory)
            .EnumerateFiles($"{BackupWorkspace.SnapshotPrefix}*{BackupWorkspace.SnapshotExtension}")
            .OrderByDescending(f => f.LastWriteTimeUtc)
            .Skip(MaxSnapshots)
            .ToArray();

        foreach (var file in stale)
            try
            {
                file.Delete();
                Report(tenantDirectory, $"Pruned old snapshot {file.Name}.");
            }
            catch (IOException)
            {
                // Keeping an extra snapshot is harmless.
            }
    }

    private static void Report(string tenantDirectory, string message)
    {
        var line = $"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}Z {message}";

        // NLog is not configured this early, and the container log is where an
        // operator will look after a restart.
        Console.WriteLine($"[backup] {line}");

        try
        {
            Directory.CreateDirectory(BackupWorkspace.GetDirectory(tenantDirectory));
            File.AppendAllText(BackupWorkspace.GetRestoreLogPath(tenantDirectory), line + System.Environment.NewLine);
        }
        catch (IOException)
        {
            // The console line is enough.
        }
    }
}
