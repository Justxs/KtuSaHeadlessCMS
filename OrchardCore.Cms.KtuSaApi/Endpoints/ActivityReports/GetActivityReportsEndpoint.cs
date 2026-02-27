using FastEndpoints;
using OrchardCore.Cms.KtuSaModule.Constants;
using OrchardCore.Cms.KtuSaModule.Extensions;
using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.ActivityReports;

public class GetActivityReportsEndpoint(IRepository repository)
    : Endpoint<GetActivityReportsRequest, List<ActivityReportResponse>>
{
    public override void Configure()
    {
        Get("api/{language}/ActivityReports/SaUnits/{saUnit}");
        AllowAnonymous();
        Description(b => b
            .WithTags("Activity Reports")
            .WithSummary("Get activity reports for an SA unit")
            .WithDescription(
                "Returns activity reports for the specified SA unit ordered by report period start date descending. Language: 'lt' or 'en'. Allowed saUnit values: CSA, InfoSA, Vivat_Chemija, InDi, STATIUS, FUMSA, ESA, SHM, VFSA, BRK.")
            .Produces<List<ActivityReportResponse>>(200));
    }

    public override async Task HandleAsync(GetActivityReportsRequest req, CancellationToken ct)
    {
        var activityReports = await repository.GetAllAsync(ContentTypeConstants.ActivityReport);
        var saUnitItem = await repository.GetSaUnitByName(req.SaUnit);
        var isLithuanian = req.Language.IsLtLanguage();

        var activityReportsDto = activityReports
            .Where(item => item.As<ActivityReportPart>().SaUnit.ContentItemIds.Contains(saUnitItem.ContentItemId))
            .OrderByDescending(item => item.As<ActivityReportPart>().From)
            .Select(item => item.ToResponse(isLithuanian))
            .ToList();

        await Send.OkAsync(activityReportsDto, ct);
    }
}