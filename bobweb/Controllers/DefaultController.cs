using bobdomain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace bobweb.Controllers
{
    [Route("api/items")]
    public class DefaultController : ControllerBase
    {
        private readonly IItemStoragePathProvider _storagePathProvider;
        private readonly ILogger<DefaultController> _logger;
        private readonly AppSettings _appSettings;

        public DefaultController(IItemStoragePathProvider storagePathProvider, ILogger<DefaultController> logger, IOptions<AppSettings> appSettings)
        {
            _storagePathProvider = storagePathProvider;
            _logger = logger;
            _appSettings = appSettings.Value ?? new AppSettings();
        }

        [HttpGet("my_list")]
        public object MyList(int id)
        {
            try
            {
                return new { Success = true, MyList = MyItems.GetMyItems(_storagePathProvider.GetDataPath(), id) };
            }
            catch (System.Exception excp)
            {
                _logger.LogError(excp, "Failed to load saved Bob items for id {Id}.", id);
                BobErrors.LogError(_appSettings, excp, "Failed to load Bob items for id {0}.", id);
            }

            return new { Success = false };
        }
    }
}
