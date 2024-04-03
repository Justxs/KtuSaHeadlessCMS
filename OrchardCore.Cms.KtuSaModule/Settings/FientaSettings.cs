using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrchardCore.Cms.KtuSaModule.Settings;

public class FientaSettings
{
    public string BaseUrl { get; set; } = string.Empty;

    public string OrganiserId { get; set; } = string.Empty;

    public string SectionName { get; } = "Fienta";
}