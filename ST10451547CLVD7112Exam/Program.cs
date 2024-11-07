using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Serilog;
using ST10451547CLVD7112Exam.Common;
using ST10451547CLVD7112Exam.Data;
using ST10451547CLVD7112Exam.Data.DataStore;

namespace BasalatAssessment.Vehicle;

public static class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .MinimumLevel.Information()
            .CreateLogger();

        try
        {
            Log.Information("Starting application");

            var builder = WebApplication.CreateBuilder(args);

            ConfigureServices(builder);

            var app = builder.Build();
            ConfigurePipeline(app);

            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        builder.Services.Configure<AppSettings>(builder.Configuration);
        var appSettings = builder.Configuration.Get<AppSettings>();
        ConfigureData(builder.Services, appSettings?.ConnectionStrings?.HealthCheckDbConnection);
        // ConfigureServices(builder.Services, builder.Configuration);
        builder.Services.AddHealthChecks();
     
        ConfigureHsts(builder.Services);
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
    }

    private static void ConfigurePipeline(WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        app.UseStaticFiles();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapDefaultControllerRoute();
        });
    }

    private static void ConfigureData(IServiceCollection services, string? VehicleTrackingConnectionString)
    {
        if (VehicleTrackingConnectionString == null)
        {
            throw new ArgumentNullException(nameof(VehicleTrackingConnectionString));
        }

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(VehicleTrackingConnectionString);
        });

        services.AddScoped<IDataStore, DataStore>();
    }

    private static void ConfigureHsts(IServiceCollection services)
    {
        services.AddHsts(options =>
        {
            options.Preload = true;
            options.IncludeSubDomains = true;
            options.MaxAge = TimeSpan.FromSeconds(63072000);
        });
    }
}
