using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;

namespace CompanyEmployees.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        private ILoggerManager _loggerManager;

        private IRepositoryManager _repositoryManager;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ILoggerManager loggerManager, IRepositoryManager repositoryManager)
        {
            _logger = logger;
            _loggerManager = loggerManager;
            _repositoryManager = repositoryManager;
        }

        [Route("weatherReport")]
        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();

            _loggerManager.LogInfo("Here is info message from our values controller.");
            _loggerManager.LogDebug("Here is debug message from our values controller.");
            _loggerManager.LogWarn("Here is warn message from our values controller.");
            _loggerManager.LogError("Here is an error message from our values controller.");

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [Route("dbops")]
        [HttpGet]
        public IActionResult GetDatabaseOperation()
        {
            var companies = _repositoryManager.Company.GetAllCompanies(false);

            return Ok(companies);
        }
    }
}
