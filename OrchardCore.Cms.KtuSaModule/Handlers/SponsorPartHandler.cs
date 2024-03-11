using OrchardCore.Cms.KtuSaModule.Models;
using OrchardCore.ContentManagement.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrchardCore.Cms.KtuSaModule.Handlers;

public class SponsorPartHandler : ContentPartHandler<SponsorPart>
{
    public override Task UpdatedAsync(UpdateContentContext context, SponsorPart instance)
    {
        context.ContentItem.DisplayText = instance.Name;

        return Task.CompletedTask;
    }

    public override Task CreatedAsync(CreateContentContext context, SponsorPart instance)
    {
        context.ContentItem.DisplayText = instance.Name;

        return Task.CompletedTask;
    }
}