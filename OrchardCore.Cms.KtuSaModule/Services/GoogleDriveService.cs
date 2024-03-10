using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using OrchardCore.Cms.KtuSaModule.ViewModels;
using static Google.Apis.Drive.v3.FilesResource;

namespace OrchardCore.Cms.KtuSaModule.Services;

public class GoogleDriveService : IGoogleDriveService
{
    private readonly DriveService _service;
    private const string CredentialsFilePath = "../OrchardCore.Cms.KtuSaModule/GoogleCredentials.json";
    private const string ImageFolder = "153RaNKRLtyEiPO95PpH2q4Jh-_jm_Djh";

    public GoogleDriveService()
    {

        GoogleCredential googleCredential;
        using (var stream = new FileStream(CredentialsFilePath, FileMode.Open, FileAccess.Read))
        {
            googleCredential = GoogleCredential.FromStream(stream)
                .CreateScoped(DriveService.Scope.Drive);
        }

        _service = new DriveService(new BaseClientService.Initializer
        {
            HttpClientInitializer = googleCredential,
            ApplicationName = "Google Drive uploads",
        });
    }

    public async Task<string> UploadImageAsync(ImageUploadFieldViewModel viewModel)
    {
        if (viewModel.FileId is not null)
        {
            await DeleteResourceAsync(viewModel.FileId);
        }

        var fileMetadata = new Google.Apis.Drive.v3.Data.File
        {
            Name = viewModel.UploadedFile!.FileName,
            MimeType = viewModel.UploadedFile.ContentType,
            Parents = new List<string>
            {
                ImageFolder,
            },
        };

        CreateMediaUpload request;
        await using (var memoryStream = new MemoryStream())
        {
            await viewModel.UploadedFile.CopyToAsync(memoryStream);

            request = _service.Files.Create(fileMetadata, memoryStream, fileMetadata.MimeType);
            request.Fields = "id";

            var response = await request.UploadAsync();

            if (response.Status != Google.Apis.Upload.UploadStatus.Completed)
            {
                throw response.Exception;
            }
        }

        return request.ResponseBody.Id;
    }

    private async Task DeleteResourceAsync(string fileId)
    {
        var command = _service.Files.Delete(fileId);
        await command.ExecuteAsync();
    }
}