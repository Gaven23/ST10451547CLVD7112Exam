using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using ST10451547CLVD7112Exam.Data;
using System;
using System.Threading;
using HealthCheckResult = ST10451547CLVD7112Exam.Data.HealthCheckResult;

namespace ST10451547CLVD7112Exam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthCheckApiController : ControllerBase
    {
        private readonly IDataStore _dataStore;

        private readonly ILogger<HealthCheckApiController> _logger;

        public HealthCheckApiController(ILogger<HealthCheckApiController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetHealthChecks")]
        public async Task<IActionResult> Get()
        {

            try
            {
                _logger.LogInformation("Starting to fetch health check results.");

                var result = await _dataStore.GetAsync();

                _logger.LogInformation("Fetched {Count} health check results.", result.Count());

       
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching health check results.");
                return StatusCode(500, "Internal server error");
            }
        }



        [HttpPost(Name = "SaveHealthCheck")]
        public async Task<IActionResult> SaveHealthCheck([FromBody] HealthCheckResult healthCheckResult)
        {
            if (healthCheckResult == null)
            {
                _logger.LogWarning("Received a null health check result.");
                return BadRequest("Health check result cannot be null.");
            }

            try
            {
                _logger.LogInformation("Saving a new health check result.");
                await _dataStore.SaveHealthAsync(healthCheckResult);

                _logger.LogInformation("Health check result saved successfully with ID: {Id}.", healthCheckResult.Healthy);


                return CreatedAtAction(nameof(Get), new { id = healthCheckResult.Healthy }, healthCheckResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving the health check result.");
                return StatusCode(500, "Internal server error");
            }
           
        }
    }
}

