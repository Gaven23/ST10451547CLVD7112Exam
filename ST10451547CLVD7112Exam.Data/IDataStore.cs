namespace ST10451547CLVD7112Exam.Data
{
    public interface IDataStore
    {
        Task<IEnumerable<HealthCheckResult>> GetAsync(CancellationToken cancellationToken = default);
        Task SaveHealthAsync(HealthCheckResult healthCheckResult);
    }
}
