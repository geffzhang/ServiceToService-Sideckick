﻿using System;
using System.Collections.Generic;
using System.Linq;
using Man.Dapr.Sidekick;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WeatherForecastService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IDaprSidecarHost _daprSidecarHost;


        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IDaprSidecarHost daprSidecarHost)
        {
            _logger = logger;
            _daprSidecarHost = daprSidecarHost;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("DaprStatus")]
        public ActionResult GetDaprStatus() => Ok(new
        {
            process = _daprSidecarHost.GetProcessInfo(),   // Information about the sidecar process such as if it is running
            options = _daprSidecarHost.GetProcessOptions() // The sidecar options if running, including ports and locations
        });
    }
}
