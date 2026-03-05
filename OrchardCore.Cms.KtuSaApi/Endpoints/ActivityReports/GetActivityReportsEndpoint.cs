using FastEndpoints;
using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;
using OrchardCore.Media;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.ActivityReports;

public class GetActivityReportsEndpoint(IRepository repository, IMediaFileStore mediaFileStore)
    : Endpoint<GetActivityReportsRequest, List<ActivityReportResponse>>
{
    public override void Configure()
    {
        Get("api/sa-units/{saUnit}/activity-reports");
        AllowAnonymous();
        Description(b => b
            .WithTags("Activity Reports")
            .WithSummary("List activity reports for an SA unit")
            .WithDescription(
                "Returns published activity reports for the specified SA unit, ordered by report period start date descending. " +
                "Use query parameter language=en (default) or language=lt. If the SA unit does not exist, an empty array is returned.")
            .Produces<List<ActivityReportResponse>>(200)
            .ProducesProblem(400));
    }

    public override async Task HandleAsync(GetActivityReportsRequest req, CancellationToken ct)
    {
        var saUnitItem = await repository.GetSaUnitByNameAsync(req.SaUnit);

        if (saUnitItem is null)
        {
            await Send.OkAsync([], ct);
            return;
        }

        var activityReports = await repository.GetAllAsync(ActivityReport);
        var language = req.Language;

        var response = activityReports
            .Where(item => item.As<ActivityReportPart>().SaUnit.ContentItemIds.Contains(saUnitItem.ContentItemId))
            .OrderByDescending(item => item.As<ActivityReportPart>().From)
            .Select(item => item.ToResponse(language, mediaFileStore))
            .ToList();

        await Send.OkAsync(response, ct);
    }
}