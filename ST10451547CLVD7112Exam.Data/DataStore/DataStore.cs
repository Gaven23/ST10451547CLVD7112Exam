using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10451547CLVD7112Exam.Data.DataStore
{
    public partial class DataStore : IDataStore
    {
        private readonly ApplicationDbContext _dbContext;

        public DataStore(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<HealthCheckResult>> GetAsync(CancellationToken cancellationToken = default)
        {
            return  _dbContext.HealthCheckResult.ToList();
        }

        public async Task SaveHealthAsync(HealthCheckResult  healthCheckResult)
        {
            var newlineItem1 = new HealthCheckResult
            {
                Degraded = healthCheckResult.Degraded,
                Healthy = healthCheckResult.Healthy,
                Unhealthy = healthCheckResult.Unhealthy,
            };

            _dbContext.HealthCheckResult.Add(newlineItem1);

            await _dbContext.SaveChangesAsync();
        }
    }
}
