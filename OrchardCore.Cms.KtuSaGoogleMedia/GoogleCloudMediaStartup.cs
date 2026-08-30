using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrchardCore.Cms.KtuSaGoogleMedia.Media.GoogleCloud;
using OrchardCore.Environment.Shell;
using OrchardCore.Environment.Shell.Configuration;
using OrchardCore.FileStorage;
using OrchardCore.Media;
using OrchardCore.Media.Core;
using OrchardCore.Media.Events;
using OrchardCore.Modules;

namespace OrchardCore.Cms.KtuSaGoogleMedia;

public sealed class GoogleCloudMediaStartup(
    ILogger<GoogleCloudMediaStartup> logger,
    IShellConfiguration shellConfiguration,
    IConfiguration hostConfiguration)
    : StartupBase
{
    public override int Order => 1000;

    public override void ConfigureServices(IServiceCollection services)
    {
        var storageOptions = GetStorageOptions();
        if (!storageOptions.IsConfigured(out var reason))
        {
            if (storageOptions.RequireGoogleCloudStorage)
                throw new InvalidOperationException(
                    $"Google Cloud media storage is required but not configured: {reason} " +
                    $"Configure section '{GoogleCloudMediaStorageOptions.SectionName}'.");

            logger.LogInformation(
                "Google Cloud media storage is not active. {Reason} Configure section '{Section}'",
                reason,
                GoogleCloudMediaStorageOptions.SectionName);

            return;
        }

        services.AddSingleton(Options.Create(storageOptions));

        services.Replace(ServiceDescriptor.Singleton<IFileStore>(serviceProvider =>
        {
            var googleOptions = serviceProvider.GetRequiredService<IOptions<GoogleCloudMediaStorageOptions>>().Value;
            var clock = serviceProvider.GetRequiredService<IClock>();
            var contentTypeProvider = serviceProvider.GetRequiredService<IContentTypeProvider>();

            return new GoogleCloudFileStore(googleOptions, clock, contentTypeProvider);
        }));

        services.Replace(ServiceDescriptor.Singleton<IMediaFileProvider>(serviceProvider =>
        {
            var mediaOptions = serviceProvider.GetRequiredService<IOptions<MediaOptions>>().Value;
            var mediaFileStore = serviceProvider.GetRequiredService<IMediaFileStore>();
            var mediaFileLogger = serviceProvider.GetRequiredService<ILogger<GoogleCloudMediaFileProvider>>();
            return new GoogleCloudMediaFileProvider(mediaOptions.AssetsRequestPath, mediaFileStore, mediaFileLogger);
        }));

        services.Replace(ServiceDescriptor.Singleton<IMediaFileStore>(serviceProvider =>
        {
            var shellSettings = serviceProvider.GetRequiredService<ShellSettings>();
            var mediaOptions = serviceProvider.GetRequiredService<IOptions<MediaOptions>>().Value;
            var googleOptions = serviceProvider.GetRequiredService<IOptions<GoogleCloudMediaStorageOptions>>().Value;
            var fileStore = serviceProvider.GetRequiredService<IFileStore>();
            var mediaEventHandlers = serviceProvider.GetServices<IMediaEventHandler>();
            var mediaCreatingEventHandlers = serviceProvider.GetServices<IMediaCreatingEventHandler>();
            var logger = serviceProvider.GetRequiredService<ILogger<DefaultMediaFileStore>>();
            var cdnBaseUrl = ResolveCdnBaseUrl(mediaOptions.CdnBaseUrl, googleOptions);

            var usesDirectGoogleCloudUrls = ShouldUseRootRelativePublicPaths(cdnBaseUrl, googleOptions);
            var requestBasePath = usesDirectGoogleCloudUrls
                ? string.Empty
                : "/" + fileStore.Combine(shellSettings.RequestUrlPrefix, mediaOptions.AssetsRequestPath);

            var httpContext = serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
            var originalPathBase =
                httpContext?.Features.Get<ShellContextFeature>()?.OriginalPathBase ?? PathString.Empty;
            if (!usesDirectGoogleCloudUrls && originalPathBase.HasValue)
                requestBasePath = fileStore.Combine(originalPathBase.Value, requestBasePath);

            return new DefaultMediaFileStore(
                fileStore,
                requestBasePath,
                cdnBaseUrl,
                mediaEventHandlers,
                mediaCreatingEventHandlers,
                logger);
        }));

        logger.LogWarning(
            "Google Cloud media storage enabled. BucketName: {BucketName}; BasePath: {BasePath}; PublicBaseUrl: {PublicBaseUrl}",
            storageOptions.BucketName,
            storageOptions.BasePath,
            ResolveCdnBaseUrl(string.Empty, storageOptions));
    }

    private GoogleCloudMediaStorageOptions GetStorageOptions()
    {
        var options = new GoogleCloudMediaStorageOptions();

        if (TryBindFromConfiguration(shellConfiguration, options)) return options;

        TryBindFromConfiguration(hostConfiguration, options);

        return options;
    }

    private static bool TryBindFromConfiguration(IConfiguration configuration, GoogleCloudMediaStorageOptions options)
    {
        var section = configuration.GetSection(GoogleCloudMediaStorageOptions.SectionName);
        if (!section.Exists()) return false;
        section.Bind(options);

        return true;
    }

    private static string ResolveCdnBaseUrl(string currentCdnBaseUrl, GoogleCloudMediaStorageOptions options)
    {
        var baseUrl = !string.IsNullOrWhiteSpace(options.PublicBaseUrl)
            ? options.PublicBaseUrl
            : !string.IsNullOrWhiteSpace(currentCdnBaseUrl)
            ? currentCdnBaseUrl
            : $"https://storage.googleapis.com/{options.BucketName}";

        baseUrl = baseUrl.TrimEnd('/');
        var basePath = (options.BasePath ?? string.Empty).Trim('/');

        if (string.IsNullOrEmpty(basePath) ||
            baseUrl.EndsWith("/" + basePath, StringComparison.OrdinalIgnoreCase))
            return baseUrl;

        return $"{baseUrl}/{basePath}";
    }

    private static bool ShouldUseRootRelativePublicPaths(string cdnBaseUrl, GoogleCloudMediaStorageOptions options)
    {
        if (string.IsNullOrWhiteSpace(cdnBaseUrl) ||
            !Uri.TryCreate(cdnBaseUrl, UriKind.Absolute, out var uri))
            return false;

        if (!uri.Host.Equals("storage.googleapis.com", StringComparison.OrdinalIgnoreCase))
            return uri.Host.Equals(
                $"{options.BucketName}.storage.googleapis.com",
                StringComparison.OrdinalIgnoreCase);

        var pathSegments = uri.AbsolutePath
            .Trim('/')
            .Split('/', StringSplitOptions.RemoveEmptyEntries);

        return pathSegments.Length >= 1 &&
               pathSegments[0].Equals(options.BucketName, StringComparison.OrdinalIgnoreCase);
    }
}
