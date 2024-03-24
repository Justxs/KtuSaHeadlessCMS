using Microsoft.Extensions.Options;
using OrchardCore.ResourceManagement;

namespace OrchardCore.Cms.KtuSaModule;

public class ResourceManagementOptionsConfiguration : IConfigureOptions<ResourceManagementOptions>
{
    private static readonly ResourceManifest Manifest = new();

    static ResourceManagementOptionsConfiguration()
    {
        Manifest.DefineStyle("QuillCss")
            .SetUrl("https://cdn.jsdelivr.net/npm/quill@2.0.0-rc.2/dist/quill.snow.css")
            .SetCdn("https://cdn.jsdelivr.net/npm/quill@2.0.0-rc.2/dist/quill.snow.css");

        Manifest.DefineScript("QuillJs")
            .SetUrl("https://cdn.jsdelivr.net/npm/quill@2.0.0-rc.2/dist/quill.js")
            .SetCdn("https://cdn.jsdelivr.net/npm/quill@2.0.0-rc.2/dist/quill.js")
            .SetDependencies("jQuery");
    }

    public void Configure(ResourceManagementOptions options)
    {
        options.ResourceManifests.Add(Manifest);
    }

}