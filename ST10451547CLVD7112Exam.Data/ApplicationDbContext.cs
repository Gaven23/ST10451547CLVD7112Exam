using Microsoft.EntityFrameworkCore;

namespace ST10451547CLVD7112Exam.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        internal DbSet<HealthCheckResult> HealthCheckResult { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<HealthCheckResult>(entity =>
            {
                
            });


        }
    }
}
