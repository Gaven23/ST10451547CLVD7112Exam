using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using ST10451547CLVD7112Exam.Data;
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
            // Get the health check results asynchronously
            var result = await _dataStore.GetAsync();

            // Return the result wrapped in Ok(), indicating successful HTTP 200 response
            return Ok(result);
        }


        [HttpPost(Name = "SaveHealthCheck")]
        public async Task<IActionResult> SaveHealthCheck([FromBody] HealthCheckResult healthCheckResult)
        {
            if (healthCheckResult == null)
            {
                return BadRequest("Health check result cannot be null.");
            }

            // Save the health check result asynchronously
            await _dataStore.SaveHealthAsync(healthCheckResult);

            // Return CreatedAtAction to indicate that the item was created
            return CreatedAtAction(nameof(Get), new { id = healthCheckResult.Healthy}, healthCheckResult);
        }
    }
}

