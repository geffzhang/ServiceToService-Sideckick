using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Man.Dapr.Sidekick;
using Microsoft.AspNetCore.Mvc;

namespace WeatherForecastProxyService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastProxyController : ControllerBase
    {
        private readonly IWeatherForecastClient _weatherForecastClient;
        private readonly IDaprSidecarHost _daprSidecarHost;

        public WeatherForecastProxyController(IWeatherForecastClient weatherForecastClient, IDaprSidecarHost daprSidecarHost)
        {
            _weatherForecastClient = weatherForecastClient;
            _daprSidecarHost = daprSidecarHost;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get(int count)
        {

            return await _weatherForecastClient.GetWeatherForecast(count);

        }

        [HttpGet("DaprStatus")]
        public ActionResult GetDaprStatus() => Ok(new
        {
            process = _daprSidecarHost.GetProcessInfo(),   // Information about the sidecar process such as if it is running
            options = _daprSidecarHost.GetProcessOptions() // The sidecar options if running, including ports and locations
        });
    }
}
