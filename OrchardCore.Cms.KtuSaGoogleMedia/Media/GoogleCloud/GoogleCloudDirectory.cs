using OrchardCore.FileStorage;

namespace OrchardCore.Cms.KtuSaGoogleMedia.Media.GoogleCloud;

public sealed class GoogleCloudDirectory : IFileStoreEntry
{
    public GoogleCloudDirectory(string path, DateTime lastModifiedUtc)
    {
        Path = path;
        LastModifiedUtc = lastModifiedUtc;
        Name = System.IO.Path.GetFileName(Path);
        DirectoryPath = Path.Length > Name.Length
            ? Path[..^(Name.Length + 1)]
            : string.Empty;
    }

    public string Path { get; }

    public string Name { get; }

    public string DirectoryPath { get; }

    public long Length => 0;
    public DateTime LastModifiedUtc { get; }
    public bool IsDirectory => true;
}
