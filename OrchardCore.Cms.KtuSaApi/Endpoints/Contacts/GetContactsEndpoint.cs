using FastEndpoints;
using OrchardCore.Cms.KtuSaModule.Constants;
using OrchardCore.Cms.KtuSaModule.Extensions;
using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.Cms.KtuSaModule.Models.Enums;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Contacts;

public class GetContactsEndpoint(IRepository repository)
    : Endpoint<GetContactsRequest, List<ContactResponse>>
{
    public override void Configure()
    {
        Get("api/{language}/Contacts");
        AllowAnonymous();
        Description(b => b
            .WithTags("Contacts")
            .WithSummary("Get contacts for an SA unit")
            .WithDescription("Returns member contacts of the specified SA unit ordered by index. Includes name, email, photo, position and responsibilities. Language: 'lt' or 'en'. Allowed saUnit values: CSA, InfoSA, Vivat_Chemija, InDi, STATIUS, FUMSA, ESA, SHM, VFSA, BRK.")
            .Produces<List<ContactResponse>>(200));
    }

    public override async Task HandleAsync(GetContactsRequest req, CancellationToken ct)
    {
        var contacts = await repository.GetAllAsync(Contact);
        var isLithuanian = req.Language.IsLtLanguage();

        var positions = await repository.GetAllAsync(Position);
        var saUnits = await repository.GetAllAsync(ContentTypeConstants.SaUnit);
        var saUnitId = saUnits.First(unit => unit.As<SaUnitPart>().UnitName == req.SaUnit.ToString()).ContentItemId;

        var contactDtos = contacts
            .Where(item => item.As<MemberPart>().SaUnit.ContentItemIds.Contains(saUnitId))
            .Select(item =>
            {
                var memberPart = item.As<MemberPart>();

                var positionPart = positions
                    .FirstOrDefault(position => memberPart.Position.ContentItemIds.Contains(position.ContentItemId))
                    .As<PositionPart>();

                return new ContactResponse
                {
                    Id = item.ContentItemId,
                    Email = memberPart.Email,
                    Name = memberPart.Name,
                    ImageSrc = memberPart.ImageUploadField.FileId,
                    Position = isLithuanian ? positionPart.NameLt : positionPart.NameEn,
                    Responsibilities = isLithuanian ? positionPart.DescriptionLt : positionPart.DescriptionEn,
                    Index = memberPart.Index,
                };
            }).ToList();

        await Send.OkAsync(contactDtos.OrderBy(contact => contact.Index).ToList(), ct);
    }
}
