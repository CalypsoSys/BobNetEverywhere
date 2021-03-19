using bobdomain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bobweb.Controllers
{
    [Route("/")]
    public class DefaultController : ControllerBase
    {
        private readonly IOptions<AppSettings> _config;
        private readonly ILogger<DefaultController> _logger;

        public DefaultController(IOptions<AppSettings> config, ILogger<DefaultController> logger)
        {
            _config = config;
            _logger = logger;
        }

        [HttpGet]
        public RedirectResult Get()
        {
            return Redirect(new Uri("/index.html", UriKind.Relative).ToString());
        }

        [HttpGet("my_list")]
        public object MyList(int id)
        {
            try
            {
                return new { Success = true, MyList = MyItems.GetMyItems(_config.Value.FileSaveLocation, id) };
            }
            catch
            {
            }

            return new { Success = false };
        }
    }
}
