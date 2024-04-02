using OrchardCore.Cms.KtuSaModule.ViewModels.Fields;

namespace OrchardCore.Cms.KtuSaModule.Interfaces;

public interface IGoogleCloudService
{
    Task<string> UploadImageAsync(ImageUploadFieldViewModel viewModel);

    Task RemoveFileAsync(string fileName);
}