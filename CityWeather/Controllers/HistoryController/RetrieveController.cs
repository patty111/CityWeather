using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using CityWeather.History;
using System.Collections.Generic;

namespace CityWeather.Controllers.HistoryController
{
    [ApiController]
    [Route("historydata")]
    public class RetrieveController : ControllerBase
    {
        private readonly HistoryManager historyManager;

        public RetrieveController(IConfiguration configuration)
        {
            historyManager = new HistoryManager(configuration);
        }

        [HttpGet]
        public IActionResult Retrieve(int retrieve_nums)
        {
            List<HistoryObject> historyObjects = historyManager.HistoryPeek(retrieve_nums);
            List<string> convert = new List<string>();

            foreach (HistoryObject historyObject in historyObjects)
            {
                string tmp = historyObject.ToString();
                int startIdx = tmp.IndexOf("{") + 2;
                int endIdx = tmp.IndexOf("}");
                convert.Add(tmp.Substring(startIdx, endIdx - startIdx));
            }

            return Ok(convert);
        }
    }
}
