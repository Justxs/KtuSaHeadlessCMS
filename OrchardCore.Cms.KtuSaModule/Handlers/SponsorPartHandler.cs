using OrchardCore.Cms.KtuSaModule.Models.Parts;

namespace OrchardCore.Cms.KtuSaModule.Handlers;

public class SponsorPartHandler : DisplayTextPartHandler<SponsorPart>
{
    protected override string GetDisplayText(SponsorPart part) => part.Name;
}