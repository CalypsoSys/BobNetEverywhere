using bobdomain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bobweb.Controllers
{
    [Route("api/items")]
    public class DefaultController : ControllerBase
    {
        private readonly IItemStoragePathProvider _storagePathProvider;
        private readonly ILogger<DefaultController> _logger;

        public DefaultController(IItemStoragePathProvider storagePathProvider, ILogger<DefaultController> logger)
        {
            _storagePathProvider = storagePathProvider;
            _logger = logger;
        }

        [HttpGet("my_list")]
        public object MyList(int id)
        {
            try
            {
                return new { Success = true, MyList = MyItems.GetMyItems(_storagePathProvider.GetDataPath(), id) };
            }
            catch
            {
            }

            return new { Success = false };
        }
    }
}
