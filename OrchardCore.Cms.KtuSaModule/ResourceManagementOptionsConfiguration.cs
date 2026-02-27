using Microsoft.Extensions.Options;
using OrchardCore.ResourceManagement;

namespace OrchardCore.Cms.KtuSaModule;

public class ResourceManagementOptionsConfiguration : IConfigureOptions<ResourceManagementOptions>
{
    private static readonly ResourceManifest Manifest = new();

    private const string QuillCssUrl = "https://cdn.jsdelivr.net/npm/quill@2.0.0-rc.5/dist/quill.snow.css";
    private const string QuillJsUrl = "https://cdn.jsdelivr.net/npm/quill@2.0.0-rc.5/dist/quill.js";
    private const string FlatpickrCssUrl = "https://cdn.jsdelivr.net/npm/flatpickr/dist/flatpickr.min.css";
    private const string FlatpickrJsUrl = "https://cdn.jsdelivr.net/npm/flatpickr";

    static ResourceManagementOptionsConfiguration()
    {
        Manifest.DefineStyle("QuillCss")
            .SetUrl(QuillCssUrl)
            .SetCdn(QuillCssUrl);

        Manifest.DefineScript("QuillJs")
            .SetUrl(QuillJsUrl)
            .SetCdn(QuillJsUrl)
            .SetDependencies("jQuery");

        Manifest.DefineStyle("FlatpickrCss")
            .SetUrl(FlatpickrCssUrl)
            .SetCdn(FlatpickrCssUrl);

        Manifest.DefineScript("FlatpickrJs")
            .SetUrl(FlatpickrJsUrl)
            .SetCdn(FlatpickrJsUrl);
    }

    public void Configure(ResourceManagementOptions options)
    {
        options.ResourceManifests.Add(Manifest);
    }
}