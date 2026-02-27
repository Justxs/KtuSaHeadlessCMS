namespace OrchardCore.Cms.KtuSaApi.Endpoints.StaticPages;

public class StaticPageResponse
{
    [Description("HTML body of the static page in the requested language")]
    public required string Body { get; set; }
}
