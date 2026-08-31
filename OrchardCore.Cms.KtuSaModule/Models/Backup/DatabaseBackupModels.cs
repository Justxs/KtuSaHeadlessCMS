namespace OrchardCore.Cms.KtuSaModule.Models.Backup;

public sealed class DatabaseBackupStatus
{
    public bool IsSupported { get; init; }

    public string? UnsupportedReason { get; init; }

    public long DatabaseSizeBytes { get; init; }

    public DateTime? DatabaseModifiedUtc { get; init; }

    public PendingRestore? PendingRestore { get; init; }

    public IReadOnlyList<RestoreSnapshot> Snapshots { get; init; } = [];
}

public sealed class PendingRestore
{
    public string FileName { get; set; } = string.Empty;

    public long SizeBytes { get; set; }

    public DateTime UploadedUtc { get; set; }

    public string UploadedBy { get; set; } = string.Empty;
}

public sealed class RestoreSnapshot
{
    public string Id { get; init; } = string.Empty;

    public long SizeBytes { get; init; }

    public DateTime CreatedUtc { get; init; }
}

public sealed record BackupOperationResult(bool Succeeded, string Message)
{
    public static BackupOperationResult Ok(string message)
    {
        return new BackupOperationResult(true, message);
    }

    public static BackupOperationResult Fail(string message)
    {
        return new BackupOperationResult(false, message);
    }
}
