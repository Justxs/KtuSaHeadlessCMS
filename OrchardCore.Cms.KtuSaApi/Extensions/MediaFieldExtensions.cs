using OrchardCore.Media;
using OrchardCore.Media.Fields;

namespace OrchardCore.Cms.KtuSaApi.Extensions;

public static class MediaFieldExtensions
{
    extension(MediaField? mediaField)
    {
        public string ToPublicUrl(IMediaFileStore mediaFileStore)
        {
            var path = mediaField?.Paths?.FirstOrDefault(p => !string.IsNullOrWhiteSpace(p));
            if (string.IsNullOrWhiteSpace(path)) return string.Empty;

            return Uri.TryCreate(path, UriKind.Absolute, out _)
                ? path
                : mediaFileStore.MapPathToPublicUrl(path.TrimStart('/'));
        }

        public List<string> ToPublicUrls(IMediaFileStore mediaFileStore)
        {
            if (mediaField?.Paths is null or { Length: 0 })
                return [];

            return
            [
                .. mediaField.Paths
                    .Where(p => !string.IsNullOrWhiteSpace(p))
                    .Select(p => Uri.TryCreate(p, UriKind.Absolute, out _)
                        ? p
                        : mediaFileStore.MapPathToPublicUrl(p.TrimStart('/')))
            ];
        }
    }
}