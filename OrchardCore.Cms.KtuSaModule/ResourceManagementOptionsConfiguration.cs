using Microsoft.Extensions.Options;
using OrchardCore.ResourceManagement;

namespace OrchardCore.Cms.KtuSaModule;

public class ResourceManagementOptionsConfiguration : IConfigureOptions<ResourceManagementOptions>
{
    private static readonly ResourceManifest Manifest = new();

    static ResourceManagementOptionsConfiguration()
    {
        Manifest.DefineStyle("QuillCss")
            .SetUrl("https://cdn.jsdelivr.net/npm/quill@2.0.0-rc.5/dist/quill.snow.css")
            .SetCdn("https://cdn.jsdelivr.net/npm/quill@2.0.0-rc.5/dist/quill.snow.css");

        Manifest.DefineScript("QuillJs")
            .SetUrl("https://cdn.jsdelivr.net/npm/quill@2.0.0-rc.5/dist/quill.js")
            .SetCdn("https://cdn.jsdelivr.net/npm/quill@2.0.0-rc.5/dist/quill.js")
            .SetDependencies("jQuery");

        Manifest.DefineStyle("FlatpickrCss")
            .SetUrl("https://cdn.jsdelivr.net/npm/flatpickr/dist/flatpickr.min.css")
            .SetCdn("https://cdn.jsdelivr.net/npm/flatpickr/dist/flatpickr.min.css");

        Manifest.DefineScript("FlatpickrJs")
            .SetUrl("https://cdn.jsdelivr.net/npm/flatpickr")
            .SetCdn("https://cdn.jsdelivr.net/npm/flatpickr");
    }

    public void Configure(ResourceManagementOptions options)
    {
        options.ResourceManifests.Add(Manifest);
    }

}