using OrchardCore.Cms.KtuSaModule.ViewModels.Fields;

namespace OrchardCore.Cms.KtuSaModule.Services;

public interface IGoogleCloudService
{
    Task<string> UploadImageAsync(ImageUploadFieldViewModel viewModel);

    Task RemoveFileAsync(string fileName);
}