using OrchardCore.Cms.KtuSaModule.Models.Backup;

namespace OrchardCore.Cms.KtuSaModule.Interfaces;

public interface IDatabaseBackupService
{
    DatabaseBackupStatus GetStatus();

    string BuildBackupFileName();

    Task<Stream> CreateBackupAsync(CancellationToken cancellationToken = default);

    Task<BackupOperationResult> StageRestoreAsync(
        Stream content,
        string fileName,
        string userName,
        CancellationToken cancellationToken = default);

    BackupOperationResult DiscardPendingRestore();

    Stream? OpenSnapshot(string id);
}
