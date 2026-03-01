using System.Text.Json;

namespace OrchardCore.Cms.KtuSaGoogleMedia.Media.GoogleCloud;

public sealed class GoogleCloudMediaStorageOptions
{
    public const string SectionName = "OrchardCore_Media_GoogleCloudStorage";
    public const string LegacySectionName = "GoogleCredentials";

    public string BucketName { get; set; } = string.Empty;
    public string BasePath { get; set; } = string.Empty;
    public string PublicBaseUrl { get; set; } = string.Empty;
    public bool RequireGoogleCloudStorage { get; set; } = true;

    public bool UseApplicationDefaultCredentials { get; set; } = true;
    public string CredentialsJson { get; set; } = string.Empty;

    public string Type { get; set; } = "service_account";
    public string ProjectId { get; set; } = string.Empty;
    public string PrivateKeyId { get; set; } = string.Empty;
    public string PrivateKey { get; set; } = string.Empty;
    public string ClientEmail { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string AuthUri { get; set; } = "https://accounts.google.com/o/oauth2/auth";
    public string TokenUri { get; set; } = "https://oauth2.googleapis.com/token";
    public string AuthProviderX509CertUrl { get; set; } = "https://www.googleapis.com/oauth2/v1/certs";
    public string ClientX509CertUrl { get; set; } = string.Empty;
    public string UniverseDomain { get; set; } = "googleapis.com";

    public bool HasInlineCredentials()
    {
        return !string.IsNullOrWhiteSpace(CredentialsJson) || HasServiceAccountFields();
    }

    public bool HasServiceAccountFields()
    {
        return !string.IsNullOrWhiteSpace(ProjectId)
               && !string.IsNullOrWhiteSpace(PrivateKey)
               && !string.IsNullOrWhiteSpace(ClientEmail);
    }

    public string BuildServiceAccountJson()
    {
        return JsonSerializer.Serialize(new
        {
            type = Type,
            project_id = ProjectId,
            private_key_id = PrivateKeyId,
            private_key = PrivateKey.Replace("\\n", "\n"),
            client_email = ClientEmail,
            client_id = ClientId,
            auth_uri = AuthUri,
            token_uri = TokenUri,
            auth_provider_x509_cert_url = AuthProviderX509CertUrl,
            client_x509_cert_url = ClientX509CertUrl,
            universe_domain = UniverseDomain
        });
    }

    public bool IsConfigured(out string reason)
    {
        if (string.IsNullOrWhiteSpace(BucketName))
        {
            reason = "BucketName is missing.";
            return false;
        }

        if (!UseApplicationDefaultCredentials && !HasInlineCredentials())
        {
            reason = "No credentials were configured and UseApplicationDefaultCredentials is disabled.";
            return false;
        }

        reason = string.Empty;
        return true;
    }
}
