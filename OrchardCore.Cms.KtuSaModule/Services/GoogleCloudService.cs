using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using OrchardCore.Cms.KtuSaModule.ViewModels;
using System;

namespace OrchardCore.Cms.KtuSaModule.Services;

public class GoogleCloudService : IGoogleCloudService
{
    private readonly StorageClient _storageClient;
    private const string CredentialsFilePath = "../OrchardCore.Cms.KtuSaModule/GoogleCredentials.json";
    private const string BucketName = "testktusa";

    public GoogleCloudService()
    {

        GoogleCredential googleCredential;
        using (var stream = new FileStream(CredentialsFilePath, FileMode.Open, FileAccess.Read))
        {
            googleCredential = GoogleCredential.FromStream(stream);
        }

        _storageClient = StorageClient.Create(googleCredential);
    }

    public async Task<string> UploadImageAsync(ImageUploadFieldViewModel viewModel)
    {
        if (viewModel.FileId is not null)
        {
            await RemoveFileAsync(viewModel.FileId);
        }

        var fileName = $"{Guid.NewGuid()}-{viewModel.UploadedFile.FileName}";
        var contentType = viewModel.UploadedFile.ContentType;

        await using var stream = viewModel.UploadedFile.OpenReadStream();

        var storageObject = await _storageClient.UploadObjectAsync(
            bucket: BucketName,
            objectName: fileName,
            contentType: contentType,
            source: stream
        );

        return storageObject.MediaLink;
    }

    public async Task RemoveFileAsync(string fileName)
    {
        var lastIndex = fileName.LastIndexOf('/');
        fileName = lastIndex != -1
            ? fileName[(lastIndex + 1)..]
            : fileName;

        var queryIndex = fileName.IndexOf('?');

        if (queryIndex != -1)
        {
            fileName = fileName[..queryIndex];
        }

        await _storageClient.DeleteObjectAsync(BucketName, fileName);
    }
}