using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrchardCore.Admin;
using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.Cms.KtuSaModule.Permissions;
using OrchardCore.Cms.KtuSaModule.Services;

namespace OrchardCore.Cms.KtuSaModule.AdminControllers;

[Admin]
public class BackupAdminController(
    IDatabaseBackupService backupService,
    IAuthorizationService authorizationService,
    IHostApplicationLifetime applicationLifetime,
    ILogger<BackupAdminController> logger) : Controller
{
    private const long MaxUploadBytes = 2L * 1024 * 1024 * 1024;

    private const string StatusMessageKey = "BackupStatusMessage";

    private const string StatusSuccessKey = "BackupStatusSuccess";

    [HttpGet]
    [Route("Admin/Backup")]
    public async Task<IActionResult> Index()
    {
        if (!await authorizationService.AuthorizeAsync(User, BackupPermissions.ManageBackups)) return Forbid();

        return View(backupService.GetStatus());
    }

    [HttpPost]
    [Route("Admin/Backup/Download")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Download(CancellationToken cancellationToken)
    {
        if (!await authorizationService.AuthorizeAsync(User, BackupPermissions.ManageBackups)) return Forbid();

        try
        {
            var stream = await backupService.CreateBackupAsync(cancellationToken);

            logger.LogInformation("Database backup downloaded by {User}.", User.Identity?.Name);

            return File(stream, "application/x-sqlite3", backupService.BuildBackupFileName());
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Creating a database backup failed.");

            return Failed($"Backup failed: {exception.Message}");
        }
    }

    [HttpPost]
    [Route("Admin/Backup/Upload")]
    [ValidateAntiForgeryToken]
    [RequestSizeLimit(MaxUploadBytes)]
    [RequestFormLimits(MultipartBodyLengthLimit = MaxUploadBytes)]
    public async Task<IActionResult> Upload(IFormFile? backupFile, bool confirmed, CancellationToken cancellationToken)
    {
        if (!await authorizationService.AuthorizeAsync(User, BackupPermissions.ManageBackups)) return Forbid();

        if (!confirmed) return Failed("Tick the confirmation box before uploading a backup.");

        if (backupFile is null || backupFile.Length == 0) return Failed("Choose a backup file to upload.");

        await using var stream = backupFile.OpenReadStream();

        var result = await backupService.StageRestoreAsync(
            stream,
            backupFile.FileName,
            User.Identity?.Name ?? "unknown",
            cancellationToken);

        return Completed(result.Succeeded, result.Message);
    }

    [HttpPost]
    [Route("Admin/Backup/Discard")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Discard()
    {
        if (!await authorizationService.AuthorizeAsync(User, BackupPermissions.ManageBackups)) return Forbid();

        var result = backupService.DiscardPendingRestore();

        return Completed(result.Succeeded, result.Message);
    }

    [HttpPost]
    [Route("Admin/Backup/Apply")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Apply()
    {
        if (!await authorizationService.AuthorizeAsync(User, BackupPermissions.ManageBackups)) return Forbid();

        var status = backupService.GetStatus();
        if (status.PendingRestore is null) return Failed("There is no staged backup to apply.");

        logger.LogWarning(
            "Shutting down to apply a staged database restore, requested by {User}.",
            User.Identity?.Name);

        // Give the response time to reach the browser, then stop. Whatever supervises
        // the process — Docker's restart policy, systemd, App Service — starts it again,
        // and PendingRestoreApplier swaps the file on the way up.
        _ = Task.Run(async () =>
        {
            await Task.Delay(TimeSpan.FromSeconds(2));
            applicationLifetime.StopApplication();
        });

        return Completed(
            true,
            "Shutting down to apply the restore. The CMS will be back in under a minute if it is "
            + "supervised by Docker, systemd or App Service. If it is not, start it again by hand.");
    }

    [HttpPost]
    [Route("Admin/Backup/Snapshot")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Snapshot(string id)
    {
        if (!await authorizationService.AuthorizeAsync(User, BackupPermissions.ManageBackups)) return Forbid();

        if (!BackupWorkspace.IsSnapshotId(id)) return BadRequest();

        var stream = backupService.OpenSnapshot(id);
        if (stream is null) return NotFound();

        return File(stream, "application/x-sqlite3", id);
    }

    private IActionResult Failed(string message)
    {
        return Completed(false, message);
    }

    private IActionResult Completed(bool succeeded, string message)
    {
        TempData[StatusSuccessKey] = succeeded;
        TempData[StatusMessageKey] = message;

        return RedirectToAction(nameof(Index));
    }
}
