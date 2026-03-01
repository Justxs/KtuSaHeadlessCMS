using OrchardCore.FileStorage;

namespace OrchardCore.Cms.KtuSaGoogleMedia.Media.GoogleCloud;

public sealed class GoogleCloudFile : IFileStoreEntry
{
    public GoogleCloudFile(string path, long length, DateTime lastModifiedUtc)
    {
        Path = path;
        Name = System.IO.Path.GetFileName(Path);
        DirectoryPath = Name == Path
            ? string.Empty
            : Path[..^(Name.Length + 1)];
        Length = length;
        LastModifiedUtc = lastModifiedUtc;
    }

    public string Path { get; }

    public string Name { get; }

    public string DirectoryPath { get; }

    public long Length { get; }
    public DateTime LastModifiedUtc { get; }
    public bool IsDirectory => false;
}
