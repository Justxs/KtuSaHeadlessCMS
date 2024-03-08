using OrchardCore.Cms.KtuSaModule.ViewModels;

namespace OrchardCore.Cms.KtuSaModule.Services;

public interface IGoogleDriveService
{
    Task UploadFile(ImageUploadFieldViewModel viewModel);
}