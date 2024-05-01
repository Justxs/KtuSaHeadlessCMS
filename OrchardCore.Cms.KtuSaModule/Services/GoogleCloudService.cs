using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Newtonsoft.Json;
using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.Cms.KtuSaModule.Settings;
using OrchardCore.Cms.KtuSaModule.ViewModels.Fields;

namespace OrchardCore.Cms.KtuSaModule.Services;

public class GoogleCloudService : IGoogleCloudService
{
    private readonly StorageClient _storageClient;
    private readonly string _bucketName;

    public GoogleCloudService(GoogleCloudSettings settings)
    {

        var jsonCredentials = JsonConvert.SerializeObject(
            new
            {
                type = settings.Type,
                project_id = settings.ProjectId,
                private_key_id = settings.PrivateKeyId,
                private_key = settings.PrivateKey.Replace("\\n", "\n"),
                client_email = settings.ClientEmail,
                client_id = settings.ClientId,
                auth_uri = settings.AuthUri,
                token_uri = settings.TokenUri,
                auth_provider_x509_cert_url = settings.AuthProviderX509CertUrl,
                client_x509_cert_url = settings.ClientX509CertUrl,
            });

        var googleCredential = GoogleCredential.FromJson(jsonCredentials);

        _storageClient = StorageClient.Create(googleCredential);
        _bucketName = settings.BucketName;
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
            bucket: _bucketName,
            objectName: fileName,
            contentType: contentType,
            source: stream
        );

        return storageObject.MediaLink;
    }

    public async Task<string> UploadPdfAsync(PdfUploadFieldViewModel viewModel)
    {
        if (viewModel.FileId is not null)
        {
            await RemoveFileAsync(viewModel.FileId);
        }

        var fileName = $"{Guid.NewGuid()}-{viewModel.UploadedFile.FileName.Replace(" ", "")}";
        var contentType = viewModel.UploadedFile.ContentType;

        await using var stream = viewModel.UploadedFile.OpenReadStream();

        var storageObject = await _storageClient.UploadObjectAsync(
            bucket: _bucketName,
            objectName: fileName,
            contentType: contentType,
            source: stream
        );

        var lastIndex = storageObject.Id.LastIndexOf('/');

        return "https://storage.googleapis.com/" + storageObject.Id[..lastIndex];
    }

    public async Task RemoveFileAsync(string fileName)
    {
        try
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

            await _storageClient.DeleteObjectAsync(_bucketName, fileName);
        }
        catch
        {
            // TODO add logging
            Console.WriteLine("Failed to delete image");
        }
    }
}