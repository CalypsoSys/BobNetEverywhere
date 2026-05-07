using bobdomain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace bobweb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChartController : ControllerBase
    {
        private readonly IItemStoragePathProvider _storagePathProvider;
        private readonly ILogger<ChartController> _logger;
        private readonly AppSettings _appSettings;

        public ChartController(IItemStoragePathProvider storagePathProvider, ILogger<ChartController> logger, IOptions<AppSettings> appSettings)
        {
            _storagePathProvider = storagePathProvider;
            _logger = logger;
            _appSettings = appSettings.Value ?? new AppSettings();
        }

        [HttpGet("save_items")]
        public object SaveItems(int id, string item_one, string item_two)
        {
            try 
            { 
                GraphData graph = new GraphData();
                    
                return new { Success = MyItems.SaveItem(_storagePathProvider.GetDataPath(), id, item_one, item_two) };
            }
            catch(Exception excp)
            {
                _logger.LogError(excp, "Failed to save Bob items for id {Id}.", id);
                BobErrors.LogError(_appSettings, excp, "Failed to save Bob items for id {0}.", id);
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
                _logger.LogError(excp, "Failed to build Bob chart data for chart type {ChartType}.", chart_type);
                BobErrors.LogError(_appSettings, excp, "Failed to build Bob chart data for chart type {0}.", chart_type);
                return new { Success = false, Error = excp.Message };
            }
        }
    }
}
