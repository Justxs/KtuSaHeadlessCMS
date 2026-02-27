namespace OrchardCore.Cms.KtuSaApi.Endpoints.Events;

public class GetEventByIdRequest
{
    [Description("Language code: 'lt' for Lithuanian or 'en' for English")]
    public string Language { get; set; } = null!;

    [Description("Content item ID of the event")]
    public string Id { get; set; } = null!;
}