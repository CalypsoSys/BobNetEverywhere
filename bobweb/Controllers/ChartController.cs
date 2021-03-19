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
    [ApiController]
    [Route("api/[controller]")]
    public class ChartController : ControllerBase
    {
        private readonly IOptions<AppSettings> _config;
        private readonly ILogger<ChartController> _logger;

        public ChartController(IOptions<AppSettings> config, ILogger<ChartController> logger)
        {
            _config = config;
            _logger = logger;
        }

        [HttpGet("save_items")]
        public object SaveItems(int id, string item_one, string item_two)
        {
            try 
            { 
                GraphData graph = new GraphData();
                    
                return new { Success = MyItems.SaveItem(_config.Value.FileSaveLocation,id, item_one, item_two) };
            }
            catch(Exception excp)
            {
                return new { Success = false, Error = excp.Message };
            }
        }

        [HttpGet("get_chart_data")]
        public object GetChartData(string chart_type)
        {
            try
            {
                GraphData graph = new GraphData();

                return new { Success = true, ChartType = chart_type, ChartData = graph.BuildGraphData(chart_type) };
            }
            catch(Exception excp)
            {
                return new { Success = false, Error = excp.Message };
            }
        }
    }
}
