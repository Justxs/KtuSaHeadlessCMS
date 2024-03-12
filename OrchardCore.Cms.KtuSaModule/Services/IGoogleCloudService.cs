using OrchardCore.Cms.KtuSaModule.ViewModels;

namespace OrchardCore.Cms.KtuSaModule.Services;

public interface IGoogleCloudService
{
    Task<string> UploadImageAsync(ImageUploadFieldViewModel viewModel);

    Task RemoveFileAsync(string fileName);
}