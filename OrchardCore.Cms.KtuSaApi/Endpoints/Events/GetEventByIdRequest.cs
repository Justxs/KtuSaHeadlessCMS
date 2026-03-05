using FastEndpoints;
using OrchardCore.Cms.KtuSaModule.Models;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Events;

public class GetEventByIdRequest
{
    [QueryParam]
    [Description("Response language. Allowed values: 'en' (default) and 'lt'.")]
    public Language Language { get; set; } = Language.EN;

    [BindFrom("id")]
    [Description("Event content item ID.")]
    public string Id { get; set; } = null!;
}
