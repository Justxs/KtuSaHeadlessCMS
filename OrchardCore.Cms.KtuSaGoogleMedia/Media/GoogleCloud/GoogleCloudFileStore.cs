using System.Net;
using Google;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.StaticFiles;
using OrchardCore.FileStorage;
using OrchardCore.Modules;
using StorageObject = Google.Apis.Storage.v1.Data.Object;

namespace OrchardCore.Cms.KtuSaGoogleMedia.Media.GoogleCloud;

public sealed class GoogleCloudFileStore(
    GoogleCloudMediaStorageOptions options,
    IClock clock,
    IContentTypeProvider contentTypeProvider)
    : IFileStore, IDisposable
{
    private const string DirectoryMarkerFileName = "OrchardCore.Media.txt";

    private static readonly byte[] MarkerFileContent =
        "This is a directory marker file used by Orchard Core."u8.ToArray();

    private readonly StorageClient _storageClient = CreateStorageClient(options);

    private readonly string _basePrefix = string.IsNullOrWhiteSpace(options.BasePath)
        ? string.Empty
        : NormalizePrefix(options.BasePath);

    public void Dispose()
    {
        _storageClient.Dispose();
    }

    public async Task<IFileStoreEntry?> GetFileInfoAsync(string path)
    {
        var normalizedPath = this.NormalizePath(path);
        if (string.IsNullOrWhiteSpace(normalizedPath)) return null;

        try
        {
            var storageObject = await _storageClient.GetObjectAsync(options.BucketName, GetObjectName(normalizedPath));
            return new GoogleCloudFile(normalizedPath, ToLength(storageObject.Size), GetLastModifiedUtc(storageObject));
        }
        catch (GoogleApiException ex) when (ex.HttpStatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }
        catch (Exception ex)
        {
            throw new FileStoreException($"Cannot get file info with path '{path}'.", ex);
        }
    }

    public async Task<IFileStoreEntry?> GetDirectoryInfoAsync(string path)
    {
        var normalizedPath = this.NormalizePath(path);

        try
        {
            if (string.IsNullOrEmpty(normalizedPath)) return new GoogleCloudDirectory(string.Empty, clock.UtcNow);

            var prefix = GetObjectPrefix(normalizedPath);
            var listOptions = new ListObjectsOptions { PageSize = 1 };

            await using var enumerator = _storageClient.ListObjectsAsync(options.BucketName, prefix, listOptions)
                .GetAsyncEnumerator();
            if (await enumerator.MoveNextAsync()) return new GoogleCloudDirectory(normalizedPath, clock.UtcNow);

            return null;
        }
        catch (Exception ex)
        {
            throw new FileStoreException($"Cannot get directory info with path '{path}'.", ex);
        }
    }

    public async IAsyncEnumerable<IFileStoreEntry> GetDirectoryContentAsync(
        string? path = null,
        bool includeSubDirectories = false)
    {
        var normalizedPath = this.NormalizePath(path ?? string.Empty);
        var requestedPrefix = string.IsNullOrEmpty(normalizedPath)
            ? string.Empty
            : normalizedPath.Trim('/') + "/";
        var objectPrefix = GetObjectPrefix(normalizedPath);

        var directories = new Dictionary<string, GoogleCloudDirectory>(StringComparer.Ordinal);
        var files = new Dictionary<string, GoogleCloudFile>(StringComparer.Ordinal);

        List<IFileStoreEntry> entries;
        try
        {
            entries = [];

            if (!includeSubDirectories)
                await ProcessNonRecursiveListingAsync(objectPrefix, requestedPrefix, directories, files);
            else
                await ProcessRecursiveListingAsync(objectPrefix, requestedPrefix, normalizedPath, directories, files);

            entries.AddRange(directories.Values.OrderBy(x => x.Path, StringComparer.OrdinalIgnoreCase)
                .Select(directory => directory));
            entries.AddRange(files.Values.OrderBy(x => x.Path, StringComparer.OrdinalIgnoreCase));
        }
        catch (Exception ex)
        {
            throw new FileStoreException($"Cannot get directory content for path '{normalizedPath}'.", ex);
        }

        foreach (var entry in entries) yield return entry;
    }

    private async Task ProcessNonRecursiveListingAsync(
        string objectPrefix,
        string requestedPrefix,
        IDictionary<string, GoogleCloudDirectory> directories,
        IDictionary<string, GoogleCloudFile> files)
    {
        var listOptions = new ListObjectsOptions { Delimiter = "/" };

        await foreach (var response in _storageClient.ListObjectsAsync(options.BucketName, objectPrefix, listOptions)
                           .AsRawResponses())
        {
            if (response.Prefixes is not null) ProcessPrefixes(response.Prefixes, requestedPrefix, directories);

            if (response.Items is not null) ProcessItems(response.Items, requestedPrefix, files, directories);
        }
    }

    private async Task ProcessRecursiveListingAsync(
        string objectPrefix,
        string requestedPrefix,
        string normalizedPath,
        IDictionary<string, GoogleCloudDirectory> directories,
        IDictionary<string, GoogleCloudFile> files)
    {
        await foreach (var storageObject in _storageClient.ListObjectsAsync(options.BucketName, objectPrefix))
        {
            var relativePath = RemoveBasePrefix(storageObject.Name);
            if (!IsValidPathForRequest(relativePath, requestedPrefix)) continue;

            var pathAfterRequest = relativePath[requestedPrefix.Length..];
            if (string.IsNullOrWhiteSpace(pathAfterRequest)) continue;

            if (relativePath.EndsWith('/'))
            {
                AddDirectory(directories, relativePath.TrimEnd('/'));
                continue;
            }

            AddAllParentDirectories(directories, normalizedPath, pathAfterRequest);

            if (IsMarkerFile(pathAfterRequest)) continue;

            files[relativePath] = new GoogleCloudFile(relativePath, ToLength(storageObject.Size),
                GetLastModifiedUtc(storageObject));
        }
    }

    private void ProcessPrefixes(
        IEnumerable<string> prefixes,
        string requestedPrefix,
        IDictionary<string, GoogleCloudDirectory> directories)
    {
        foreach (var prefix in prefixes)
        {
            var relativePrefix = RemoveBasePrefix(prefix).TrimEnd('/');
            if (string.IsNullOrWhiteSpace(relativePrefix)) continue;

            if (!relativePrefix.StartsWith(requestedPrefix, StringComparison.Ordinal)) continue;

            var childPath = relativePrefix[requestedPrefix.Length..];
            if (string.IsNullOrWhiteSpace(childPath) || childPath.Contains('/')) continue;

            AddDirectory(directories, relativePrefix);
        }
    }

    private void ProcessItems(
        IEnumerable<StorageObject> items,
        string requestedPrefix,
        IDictionary<string, GoogleCloudFile> files,
        IDictionary<string, GoogleCloudDirectory> directories)
    {
        foreach (var storageObject in items)
        {
            var relativePath = RemoveBasePrefix(storageObject.Name);
            if (!IsValidPathForRequest(relativePath, requestedPrefix)) continue;

            if (relativePath.EndsWith('/'))
            {
                AddDirectory(directories, relativePath.TrimEnd('/'));
                continue;
            }

            var childPath = relativePath[requestedPrefix.Length..];
            if (string.IsNullOrWhiteSpace(childPath) || childPath.Contains('/')) continue;

            if (IsMarkerFile(childPath)) continue;

            files[relativePath] = new GoogleCloudFile(relativePath, ToLength(storageObject.Size),
                GetLastModifiedUtc(storageObject));
        }
    }

    private static bool IsValidPathForRequest(string relativePath, string requestedPrefix)
    {
        return !string.IsNullOrWhiteSpace(relativePath)
               && relativePath.StartsWith(requestedPrefix, StringComparison.Ordinal);
    }

    private static bool IsMarkerFile(string path)
    {
        return Path.GetFileName(path).Equals(DirectoryMarkerFileName, StringComparison.Ordinal);
    }

    public async Task<bool> TryCreateDirectoryAsync(string path)
    {
        var normalizedPath = this.NormalizePath(path);
        if (string.IsNullOrWhiteSpace(normalizedPath)) return false;

        try
        {
            if (await GetFileInfoAsync(normalizedPath) is not null)
                throw new FileStoreException(
                    $"Cannot create directory because the path '{normalizedPath}' already exists and is a file.");

            if (await GetDirectoryInfoAsync(normalizedPath) is not null) return false;

            var markerPath = this.Combine(normalizedPath, DirectoryMarkerFileName);
            await using var stream = new MemoryStream(MarkerFileContent);
            await _storageClient.UploadObjectAsync(
                options.BucketName,
                GetObjectName(markerPath),
                "text/plain",
                stream);

            return true;
        }
        catch (FileStoreException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new FileStoreException($"Cannot create directory '{normalizedPath}'.", ex);
        }
    }

    public async Task<bool> TryDeleteFileAsync(string path)
    {
        var normalizedPath = this.NormalizePath(path);
        if (string.IsNullOrWhiteSpace(normalizedPath)) return false;

        try
        {
            await _storageClient.DeleteObjectAsync(options.BucketName, GetObjectName(normalizedPath));
            return true;
        }
        catch (GoogleApiException ex) when (ex.HttpStatusCode == HttpStatusCode.NotFound)
        {
            return false;
        }
        catch (Exception ex)
        {
            throw new FileStoreException($"Cannot delete file '{normalizedPath}'.", ex);
        }
    }

    public async Task<bool> TryDeleteDirectoryAsync(string path)
    {
        var normalizedPath = this.NormalizePath(path);
        if (string.IsNullOrWhiteSpace(normalizedPath))
            throw new FileStoreException("Cannot delete the root directory.");

        try
        {
            var prefix = GetObjectPrefix(normalizedPath);
            var objectNames = new List<string>();

            await foreach (var storageObject in _storageClient.ListObjectsAsync(options.BucketName, prefix))
                objectNames.Add(storageObject.Name);

            if (objectNames.Count == 0) return false;

            var deleted = false;
            foreach (var objectName in objectNames)
                try
                {
                    await _storageClient.DeleteObjectAsync(options.BucketName, objectName);
                    deleted = true;
                }
                catch (GoogleApiException ex) when (ex.HttpStatusCode == HttpStatusCode.NotFound)
                {
                    // Deleted concurrently, ignore.
                }

            return deleted;
        }
        catch (FileStoreException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new FileStoreException($"Cannot delete directory '{normalizedPath}'.", ex);
        }
    }

    public async Task MoveFileAsync(string oldPath, string newPath)
    {
        await CopyFileAsync(oldPath, newPath);
        await TryDeleteFileAsync(oldPath);
    }

    public async Task CopyFileAsync(string srcPath, string dstPath)
    {
        var sourcePath = this.NormalizePath(srcPath);
        var destinationPath = this.NormalizePath(dstPath);

        if (string.IsNullOrWhiteSpace(sourcePath))
            throw new FileStoreException("Cannot copy file because the source path is empty.");

        if (string.IsNullOrWhiteSpace(destinationPath))
            throw new FileStoreException("Cannot copy file because the destination path is empty.");

        if (sourcePath == destinationPath)
            throw new ArgumentException("The values for srcPath and dstPath must not be the same.");

        try
        {
            if (await GetFileInfoAsync(sourcePath) is null)
                throw new FileStoreException($"Cannot copy file '{sourcePath}' because it does not exist.");

            if (await GetFileInfoAsync(destinationPath) is not null)
                throw new FileStoreException(
                    $"Cannot copy file '{sourcePath}' because a file already exists in the new path '{destinationPath}'.");

            await _storageClient.CopyObjectAsync(
                options.BucketName,
                GetObjectName(sourcePath),
                options.BucketName,
                GetObjectName(destinationPath));
        }
        catch (FileStoreException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new FileStoreException($"Cannot copy file '{sourcePath}' to '{destinationPath}'.", ex);
        }
    }

    public async Task<Stream> GetFileStreamAsync(string path)
    {
        var normalizedPath = this.NormalizePath(path);
        if (string.IsNullOrWhiteSpace(normalizedPath))
            throw new FileStoreException("Cannot get file stream because the path is empty.");

        try
        {
            var stream = new MemoryStream();
            await _storageClient.DownloadObjectAsync(options.BucketName, GetObjectName(normalizedPath), stream);
            stream.Position = 0;
            return stream;
        }
        catch (GoogleApiException ex) when (ex.HttpStatusCode == HttpStatusCode.NotFound)
        {
            throw new FileStoreException($"Cannot get file stream because the file '{normalizedPath}' does not exist.");
        }
        catch (Exception ex)
        {
            throw new FileStoreException($"Cannot get file stream of the file '{normalizedPath}'.", ex);
        }
    }

    public Task<Stream> GetFileStreamAsync(IFileStoreEntry fileStoreEntry)
    {
        return GetFileStreamAsync(fileStoreEntry.Path);
    }

    public async Task<string> CreateFileFromStreamAsync(string path, Stream inputStream, bool overwrite = false)
    {
        var normalizedPath = this.NormalizePath(path);
        if (string.IsNullOrWhiteSpace(normalizedPath))
            throw new FileStoreException("Cannot create file because the path is empty.");

        try
        {
            if (!overwrite && await GetFileInfoAsync(normalizedPath) is not null)
                throw new FileStoreException($"Cannot create file '{normalizedPath}' because it already exists.");

            contentTypeProvider.TryGetContentType(normalizedPath, out var contentType);

            await _storageClient.UploadObjectAsync(
                options.BucketName,
                GetObjectName(normalizedPath),
                contentType ?? "application/octet-stream",
                inputStream);

            return normalizedPath;
        }
        catch (FileStoreException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new FileStoreException($"Cannot create file '{normalizedPath}'.", ex);
        }
    }

    private static StorageClient CreateStorageClient(GoogleCloudMediaStorageOptions options)
    {
        if (!string.IsNullOrWhiteSpace(options.CredentialsJson))
        {
            var credentialJson = options.CredentialsJson.Replace("\\n", "\n");
            return StorageClient.Create(GoogleCredential.FromJson(credentialJson));
        }

        if (options.HasServiceAccountFields())
            return StorageClient.Create(GoogleCredential.FromJson(options.BuildServiceAccountJson()));

        return options.UseApplicationDefaultCredentials
            ? StorageClient.Create()
            : throw new InvalidOperationException("No Google Cloud Storage credentials were configured.");
    }

    private string GetObjectName(string path)
    {
        var normalizedPath = this.NormalizePath(path);
        return this.Combine(_basePrefix, normalizedPath);
    }

    private string GetObjectPrefix(string path)
    {
        var normalizedPath = this.NormalizePath(path);
        return NormalizePrefix(this.Combine(_basePrefix, normalizedPath));
    }

    private string RemoveBasePrefix(string objectName)
    {
        if (string.IsNullOrEmpty(_basePrefix) || !objectName.StartsWith(_basePrefix, StringComparison.Ordinal))
            return objectName.TrimStart('/');

        // Keep trailing '/' so virtual directory placeholder objects are detected correctly.
        return objectName[_basePrefix.Length..].TrimStart('/');
    }

    private static string NormalizePrefix(string prefix)
    {
        prefix = prefix.Trim('/') + "/";
        return prefix.Length == 1 ? string.Empty : prefix;
    }

    private static long ToLength(ulong? value)
    {
        if (value is null) return 0;

        return value.Value > long.MaxValue ? long.MaxValue : (long)value.Value;
    }

    private static DateTime GetLastModifiedUtc(StorageObject storageObject)
    {
        return storageObject.UpdatedDateTimeOffset?.UtcDateTime ?? DateTime.UtcNow;
    }

    private void AddDirectory(IDictionary<string, GoogleCloudDirectory> directories, string path)
    {
        var normalized = this.NormalizePath(path);
        if (string.IsNullOrEmpty(normalized) || directories.ContainsKey(normalized)) return;

        directories[normalized] = new GoogleCloudDirectory(normalized, clock.UtcNow);
    }

    private void AddAllParentDirectories(
        IDictionary<string, GoogleCloudDirectory> directories,
        string normalizedPath,
        string pathAfterRequest)
    {
        var directoryPart = Path.GetDirectoryName(pathAfterRequest)?.Replace('\\', '/');
        if (string.IsNullOrEmpty(directoryPart)) return;

        var current = normalizedPath;
        foreach (var segment in directoryPart.Split('/', StringSplitOptions.RemoveEmptyEntries))
        {
            current = string.IsNullOrEmpty(current)
                ? segment
                : this.Combine(current, segment);
            AddDirectory(directories, current);
        }
    }
}