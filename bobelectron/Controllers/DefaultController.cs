using bobdomain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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
                return new { Success = true, MyList = MyItems.GetMyItems(id) };
            }
            catch
            {
            }

            return new { Success = false };
        }
    }
}
