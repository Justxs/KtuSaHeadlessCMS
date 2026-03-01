using OrchardCore.Cms.KtuSaModule.Models.Parts;

namespace OrchardCore.Cms.KtuSaModule.Handlers;

public class MemberPartHandler : DisplayTextPartHandler<MemberPart>
{
    protected override string GetDisplayText(MemberPart part) => part.Name;
}