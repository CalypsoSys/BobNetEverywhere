using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using System.IO;

namespace bobweb
{
    public class ItemStoragePathProvider : IItemStoragePathProvider
    {
        private readonly AppSettings _settings;
        private readonly IWebHostEnvironment _environment;

        public ItemStoragePathProvider(IOptions<AppSettings> settings, IWebHostEnvironment environment)
        {
            _settings = settings.Value;
            _environment = environment;
        }

        /// <summary>
        /// Gets the configured item storage path, defaulting to a local data folder for sample persistence.
        /// </summary>
        public string GetDataPath()
        {
            if (!string.IsNullOrWhiteSpace(_settings.FileSaveLocation))
            {
                return Path.GetFullPath(_settings.FileSaveLocation, _environment.ContentRootPath);
            }

            return Path.Combine(_environment.ContentRootPath, "data");
        }
    }
}
