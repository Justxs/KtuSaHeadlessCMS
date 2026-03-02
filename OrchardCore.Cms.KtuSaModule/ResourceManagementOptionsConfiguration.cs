using Microsoft.Extensions.Options;
using OrchardCore.Cms.KtuSaModule.Constants;
using OrchardCore.ResourceManagement;

namespace OrchardCore.Cms.KtuSaModule;

public class ResourceManagementOptionsConfiguration : IConfigureOptions<ResourceManagementOptions>
{
    private static readonly ResourceManifest Manifest = new();

    private const string FlatpickrCdnBase = "https://cdn.jsdelivr.net/npm/flatpickr";
    private const string ModuleBase = "~/OrchardCore.Cms.KtuSaModule";

    static ResourceManagementOptionsConfiguration()
    {
        // Flatpickr (CDN)
        Manifest.DefineStyle(ResourceNames.FlatpickrCss)
            .SetUrl($"{FlatpickrCdnBase}/dist/flatpickr.min.css")
            .SetCdn($"{FlatpickrCdnBase}/dist/flatpickr.min.css");

        Manifest.DefineScript(ResourceNames.FlatpickrJs)
            .SetUrl(FlatpickrCdnBase)
            .SetCdn(FlatpickrCdnBase);

        // Flatpickr field (local)
        Manifest.DefineScript(ResourceNames.FlatpickrFieldJs)
            .SetUrl($"{ModuleBase}/js/flatpickr-field.js")
            .SetDependencies(ResourceNames.FlatpickrJs);
    }

    public void Configure(ResourceManagementOptions options)
    {
        options.ResourceManifests.Add(Manifest);
    }
}