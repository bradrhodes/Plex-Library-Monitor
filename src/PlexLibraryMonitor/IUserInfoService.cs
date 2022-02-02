using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PlexLibraryMonitor.Plex.DomainModels;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace PlexLibraryMonitor
{
    public interface IUserInfoService
    {
        Task<PlexUserInfo> GetUserInfo();
        Task<PlexUserInfo> RefreshUserInfo();
    }

    class UserInfoService : IUserInfoService
    {
        private readonly IStorageAdapter _storage;

        public UserInfoService(IStorageAdapter storage)
        {
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }
        public Task<PlexUserInfo> GetUserInfo()
        {
            // First try to get all the info from cache
            // Failing that, get info from Plex
        }

        public Task<PlexUserInfo> RefreshUserInfo()
        {
            throw new System.NotImplementedException();
        }
    }

    internal interface IStorageAdapter
    {
        Task<StorageWriteResult> StoreObject(object data, CancellationToken cancellationToken);
        Task<StorageWriteResult> StoreObject(object data, CancellationToken cancellationToken, string objectName);
        Task<StorageReadResult<T>> RetrieveObject<T>(CancellationToken cancellationToken);
        Task<StorageReadResult<T>> RetrieveObject<T>(string objectName, CancellationToken cancellationToken);
    }

    public abstract record StorageReadResult<T>
    {
        public record Success(T Data) : StorageReadResult<T>;

        public record Failure() : StorageReadResult<T>;
    }

    public abstract record StorageWriteResult
    {
        public record Success() : StorageWriteResult;
        public record Failure(string Reason) : StorageWriteResult;
    }
    
    class FileStorageAdapter : IStorageAdapter
    {
        private readonly FileStorageSettings _settings;

        public FileStorageAdapter(FileStorageSettings settings)
        {
            _settings = settings;
        }

        public Task<StorageWriteResult> StoreObject(object data, CancellationToken cancellationToken)
            => StoreObject(data, cancellationToken, string.Empty);

        public async Task<StorageWriteResult> StoreObject(object data,
            CancellationToken cancellationToken, string objectName)
        {
            try
            {
                // Grab current data object, mutate it and save it again
                var jsonString = await File.ReadAllTextAsync(_settings.FullFileName, cancellationToken)
                    .ConfigureAwait(false);
                var rootNode = JsonNode.Parse(jsonString);

                var node = rootNode?[objectName];

                var serializedObject = await JsonSerializer.SerializeAsync();

                if (node == null)
                {
                    var newNode = new JsonObject(serializedObject);
                }
                    

            }
            catch (Exception ex)
            {
                return new StorageWriteResult.Failure(ex.Message);
            }
        }

        public Task<StorageReadResult<T>> RetrieveObject<T>(CancellationToken cancellationToken)
            => RetrieveObject<T>(objectName: typeof(T).Name, cancellationToken);

        public async Task<StorageReadResult<T>> RetrieveObject<T>(string objectName,
            CancellationToken cancellationToken)
        {
            var jsonString = await File.ReadAllTextAsync(_settings.FullFileName, cancellationToken).ConfigureAwait(false);;
            var jsonNode = JsonNode.Parse(jsonString);

            if (jsonNode == null)
                return new StorageReadResult<T>.Failure();

            var result = jsonNode[objectName].GetValue<T>();

            return new StorageReadResult<T>.Success(result);
        }

        private void UpsertDataFolder()
        {
            if (!Directory.Exists(_settings.FolderName))
                Directory.CreateDirectory(_settings.FolderName);
        }
    }

    public record FileStorageSettings
    {
        public string FolderName { get; init; } = string.Empty;
        public string FileName { get; init; } = string.Empty;

        public string FullFileName => $"{FolderName}{Path.DirectorySeparatorChar}{FileName}";
    };
}