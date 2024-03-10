using OrchardCore.Cms.KtuSaModule.ViewModels;

namespace OrchardCore.Cms.KtuSaModule.Services;

public interface IGoogleDriveService
{
    Task<string> UploadImageAsync(ImageUploadFieldViewModel viewModel);

    Task RemoveFileAsync(string fileId);
}