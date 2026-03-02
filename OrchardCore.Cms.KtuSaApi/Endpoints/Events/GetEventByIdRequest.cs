using FastEndpoints;
using OrchardCore.Cms.KtuSaModule.Models;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Events;

public class GetEventByIdRequest
{
    [QueryParam]
    [Description("Language code: 'lt' for Lithuanian or 'en' for English")]
    public Language Language { get; set; } = Language.EN;

    [BindFrom("id")]
    [Description("Content item ID of the event")]
    public string Id { get; set; } = null!;
}