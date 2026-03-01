using FastEndpoints;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Events;

public class GetEventByIdRequest
{
    [QueryParam]
    [AllowedValues("lt", "en")]
    [Description("Language code: 'lt' for Lithuanian or 'en' for English")]
    public string Language { get; set; } = "en";

    [BindFrom("id")]
    [Description("Content item ID of the event")]
    public string Id { get; set; } = null!;
}