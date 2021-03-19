using bobdomain;
using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace bobelectron.Controllers
{
    [Route("/")]
    public class DefaultController : ControllerBase
    {
        private readonly ILogger<DefaultController> _logger;

        public DefaultController(ILogger<DefaultController> logger)
        {
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
#if DEBUG
                System.Diagnostics.Debugger.Launch();
#endif
                var userPath = Electron.App.GetPathAsync(PathName.AppData);
                userPath.Wait();

                return new { Success = true, MyList = MyItems.GetMyItems(Path.Combine(userPath.Result, "bobelectron"), id) };
            }
            catch
            {
            }

            return new { Success = false };
        }
    }
}
