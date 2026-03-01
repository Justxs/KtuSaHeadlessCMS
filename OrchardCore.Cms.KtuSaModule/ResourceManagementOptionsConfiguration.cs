using Microsoft.Extensions.Options;
using OrchardCore.Cms.KtuSaModule.Constants;
using OrchardCore.ResourceManagement;

namespace OrchardCore.Cms.KtuSaModule;

public class ResourceManagementOptionsConfiguration : IConfigureOptions<ResourceManagementOptions>
{
    private static readonly ResourceManifest Manifest = new();

    private const string QuillVersion = "2.0.0-rc.5";
    private const string QuillCdnBase = $"https://cdn.jsdelivr.net/npm/quill@{QuillVersion}/dist";
    private const string FlatpickrCdnBase = "https://cdn.jsdelivr.net/npm/flatpickr";
    private const string ModuleBase = "~/OrchardCore.Cms.KtuSaModule";

    static ResourceManagementOptionsConfiguration()
    {
        // Quill (CDN)
        Manifest.DefineStyle(ResourceNames.QuillCss)
            .SetUrl($"{QuillCdnBase}/quill.snow.css")
            .SetCdn($"{QuillCdnBase}/quill.snow.css");

        Manifest.DefineScript(ResourceNames.QuillJs)
            .SetUrl($"{QuillCdnBase}/quill.js")
            .SetCdn($"{QuillCdnBase}/quill.js")
            .SetDependencies("jQuery");

        // Quill field (local)
        Manifest.DefineStyle(ResourceNames.QuillFieldCss)
            .SetUrl($"{ModuleBase}/css/quill-field.css")
            .SetDependencies(ResourceNames.QuillCss);

        Manifest.DefineScript(ResourceNames.QuillFieldJs)
            .SetUrl($"{ModuleBase}/js/quill-field.js")
            .SetDependencies(ResourceNames.QuillJs);

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