using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using OrchardCore.FileStorage;
using OrchardCore.Media;

namespace OrchardCore.Cms.KtuSaGoogleMedia.Media.GoogleCloud;

public sealed class GoogleCloudMediaFileProvider(
    PathString virtualPathBase,
    IMediaFileStore mediaFileStore,
    ILogger<GoogleCloudMediaFileProvider> logger) : IMediaFileProvider
{
    public PathString VirtualPathBase { get; } = virtualPathBase;

    public IFileInfo GetFileInfo(string subpath)
    {
        var normalizedPath = mediaFileStore.NormalizePath(subpath);
        if (string.IsNullOrWhiteSpace(normalizedPath))
        {
            return new NotFoundFileInfo(subpath);
        }

        try
        {
            var fileStoreEntry = mediaFileStore.GetFileInfoAsync(normalizedPath).GetAwaiter().GetResult();
            if (fileStoreEntry is null || fileStoreEntry.IsDirectory)
            {
                return new NotFoundFileInfo(normalizedPath);
            }

            return new GoogleCloudMediaFileInfo(mediaFileStore, normalizedPath, fileStoreEntry);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error resolving media file info for path '{Path}'.", normalizedPath);
            return new NotFoundFileInfo(normalizedPath);
        }
    }

    public IDirectoryContents GetDirectoryContents(string subpath)
    {
        return NotFoundDirectoryContents.Singleton;
    }

    public IChangeToken Watch(string filter)
    {
        return NullChangeToken.Singleton;
    }

    private sealed class GoogleCloudMediaFileInfo(
        IMediaFileStore mediaFileStore,
        string normalizedPath,
        IFileStoreEntry fileStoreEntry) : IFileInfo
    {
        public bool Exists => true;

        public long Length => fileStoreEntry.Length;

        public string? PhysicalPath => null;

        public string Name => fileStoreEntry.Name;

        public DateTimeOffset LastModified => ToUtcDateTimeOffset(fileStoreEntry.LastModifiedUtc);

        public bool IsDirectory => false;

        public Stream CreateReadStream()
        {
            return mediaFileStore.GetFileStreamAsync(normalizedPath).GetAwaiter().GetResult();
        }

        private static DateTimeOffset ToUtcDateTimeOffset(DateTime value)
        {
            return value.Kind switch
            {
                DateTimeKind.Utc => new DateTimeOffset(value),
                DateTimeKind.Local => value.ToUniversalTime(),
                _ => new DateTimeOffset(DateTime.SpecifyKind(value, DateTimeKind.Utc))
            };
        }
    }
}
