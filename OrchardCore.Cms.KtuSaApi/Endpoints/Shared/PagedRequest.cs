using FastEndpoints;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Shared;

public abstract class PagedRequest
{
    [QueryParam]
    [Description("1-based page number. Defaults to 1. Out-of-range values are clamped.")]
    public int? Page { get; set; }

    [QueryParam]
    [Description(
        "Number of items per page. Falls back to the endpoint default and is capped at 100.")]
    public int? PageSize { get; set; }
}
