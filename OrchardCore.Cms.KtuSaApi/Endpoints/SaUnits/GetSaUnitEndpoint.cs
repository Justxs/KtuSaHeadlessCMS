using FastEndpoints;
using OrchardCore.Cms.KtuSaModule.Constants;
using OrchardCore.Cms.KtuSaModule.Extensions;
using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.Cms.KtuSaModule.Models.Enums;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.SaUnits;

public class GetSaUnitEndpoint(IRepository repository)
    : Endpoint<GetSaUnitRequest, SaUnitResponse>
{
    public override void Configure()
    {
        Get("api/{language}/SaUnits/{saUnit}");
        AllowAnonymous();
        Description(b => b
            .WithTags("SA Units")
            .WithSummary("Get SA unit details")
            .WithDescription("Returns details for a specific student association unit, including social media links, description and contact information. Language: 'lt' or 'en'. Allowed saUnit values: CSA, InfoSA, Vivat_Chemija, InDi, STATIUS, FUMSA, ESA, SHM, VFSA, BRK.")
            .Produces<SaUnitResponse>(200)
            .ProducesProblem(404));
    }

    public override async Task HandleAsync(GetSaUnitRequest req, CancellationToken ct)
    {
        var saUnits = await repository.GetAllAsync(ContentTypeConstants.SaUnit);
        var filterSaUnit = saUnits.FirstOrDefault(item => item.As<SaUnitPart>().UnitName == req.SaUnit.ToString());

        if (filterSaUnit is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        var saUnitPart = filterSaUnit.As<SaUnitPart>();
        var contactPart = filterSaUnit.As<ContactPart>();
        var isLithuanian = req.Language.IsLtLanguage();

        var saUnitDto = new SaUnitResponse
        {
            CoverUrl = saUnitPart.SaPhoto.FileId,
            Description = isLithuanian ? saUnitPart.DescriptionLt : saUnitPart.DescriptionEn,
            LinkedInUrl = saUnitPart.LinkedInUrl,
            FacebookUrl = saUnitPart.FacebookUrl,
            InstagramUrl = saUnitPart.InstagramUrl,
            Address = saUnitPart.Address,
            Email = contactPart.Email,
            PhoneNumber = contactPart.PhoneNumber,
        };

        await Send.OkAsync(saUnitDto, ct);
    }
}
