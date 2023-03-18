using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OrchardCore.Environment.Shell;
using OrchardCore.Environment.Shell.Configuration;
using OrchardCore.Shells.Azure.Services;

namespace OrchardCore.Shells.Azure.Configuration
{
    public class BlobShellsSettingsSources : IShellsSettingsSources
    {
        private readonly IShellsFileStore _shellsFileStore;
        private readonly BlobShellStorageOptions _blobOptions;

        private readonly string _tenantsFileSystemName;

        public BlobShellsSettingsSources(IShellsFileStore shellsFileStore,
            BlobShellStorageOptions blobOptions,
            IOptions<ShellOptions> shellOptions)
        {
            _shellsFileStore = shellsFileStore;
            _blobOptions = blobOptions;
            _tenantsFileSystemName = Path.Combine(shellOptions.Value.ShellsApplicationDataPath, OrchardCoreConstants.TenantsFileName);
        }

        public async Task AddSourcesAsync(IConfigurationBuilder builder)
        {
            var fileInfo = await _shellsFileStore.GetFileInfoAsync(OrchardCoreConstants.TenantsFileName);
            if (fileInfo == null && _blobOptions.MigrateFromFiles)
            {
                if (await TryMigrateFromFileAsync())
                {
                    fileInfo = await _shellsFileStore.GetFileInfoAsync(OrchardCoreConstants.TenantsFileName);
                }
                else
                {
                    return;
                }
            }

            if (fileInfo != null)
            {
                var stream = await _shellsFileStore.GetFileStreamAsync(OrchardCoreConstants.TenantsFileName);
                builder.AddJsonStream(stream);
            }
        }

        public Task AddSourcesAsync(string tenant, IConfigurationBuilder builder) => AddSourcesAsync(builder);

        public async Task SaveAsync(string tenant, IDictionary<string, string> data)
        {
            JObject tenantsSettings;

            var fileInfo = await _shellsFileStore.GetFileInfoAsync(OrchardCoreConstants.TenantsFileName);

            if (fileInfo != null)
            {
                using (var stream = await _shellsFileStore.GetFileStreamAsync(OrchardCoreConstants.TenantsFileName))
                {
                    using (var streamReader = new StreamReader(stream))
                    {
                        using (var reader = new JsonTextReader(streamReader))
                        {
                            tenantsSettings = await JObject.LoadAsync(reader);
                        }
                    }
                }
            }
            else
            {
                tenantsSettings = new JObject();
            }

            var settings = tenantsSettings.GetValue(tenant) as JObject ?? new JObject();

            foreach (var key in data.Keys)
            {
                if (data[key] != null)
                {
                    settings[key] = data[key];
                }
                else
                {
                    settings.Remove(key);
                }
            }

            tenantsSettings[tenant] = settings;

            using (var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream))
                {
                    using (var jsonWriter = new JsonTextWriter(streamWriter) { Formatting = Formatting.Indented })
                    {
                        await tenantsSettings.WriteToAsync(jsonWriter);
                        await jsonWriter.FlushAsync();
                        memoryStream.Position = 0;
                        await _shellsFileStore.CreateFileFromStreamAsync(OrchardCoreConstants.TenantsFileName, memoryStream);
                    }
                }
            }
        }

        public async Task RemoveAsync(string tenant)
        {
            var fileInfo = await _shellsFileStore.GetFileInfoAsync(OrchardCoreConstants.TenantsFileName);

            if (fileInfo != null)
            {
                JObject tenantsSettings;
                using (var stream = await _shellsFileStore.GetFileStreamAsync(OrchardCoreConstants.TenantsFileName))
                {
                    using var streamReader = new StreamReader(stream);
                    using var reader = new JsonTextReader(streamReader);
                    tenantsSettings = await JObject.LoadAsync(reader);
                }

                tenantsSettings.Remove(tenant);

                using var memoryStream = new MemoryStream();
                using var streamWriter = new StreamWriter(memoryStream);
                using var jsonWriter = new JsonTextWriter(streamWriter) { Formatting = Formatting.Indented };

                await tenantsSettings.WriteToAsync(jsonWriter);
                await jsonWriter.FlushAsync();
                memoryStream.Position = 0;
                await _shellsFileStore.CreateFileFromStreamAsync(OrchardCoreConstants.TenantsFileName, memoryStream);
            }
        }

        private async Task<bool> TryMigrateFromFileAsync()
        {
            if (!File.Exists(_tenantsFileSystemName))
            {
                return false;
            }

            using (var file = File.OpenRead(_tenantsFileSystemName))
            {
                await _shellsFileStore.CreateFileFromStreamAsync(OrchardCoreConstants.TenantsFileName, file);
            }

            return true;
        }
    }
}
