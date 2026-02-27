namespace OrchardCore.Cms.KtuSaApi.Endpoints.Documents;

public class GetDocumentsRequest
{
    [Description("Language code: 'lt' for Lithuanian or 'en' for English")]
    public string Language { get; set; } = null!;
}
