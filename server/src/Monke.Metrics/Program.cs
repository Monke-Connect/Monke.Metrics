using Microsoft.EntityFrameworkCore;

using Monke.Metrics.Background;
using Monke.Metrics.Database;
using Monke.Metrics.Exceptions;
using Monke.Metrics.Extensions;

using Serilog;

namespace Monke.Metrics
{
	public class Program
	{
		public static void Main(string[] args)
		{
			// Bootstrap logger for the application startup
			Log.Logger = new LoggerConfiguration()
				.WriteTo.Console()
				.CreateBootstrapLogger();

			try
			{
				Log.Information("Bootstraping application.");

				WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
				_ = builder.ConfigureLogging();
				_ = builder.Services.AddControllers();
				_ = builder.Services.AddHealthChecks();
				_ = builder.Services.AddAnnotatedServices();
				_ = builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
				_ = builder.Services.AddProblemDetails();
				_ = builder.Services.AddHostedService<HardwareInfoService>();
				_ = builder.Services.ConfigureRateLimiting();
				_ = builder.Services.ConfigureDatabaseContext(builder);
				_ = builder.ConfigureNetworkHandling();

				WebApplication app = builder.Build();
				_ = app.UseSerilogRequestLogging();
				_ = app.UseExceptionHandler();
				_ = app.UseCors("Network");
				_ = app.UseRateLimiter();
				_ = app.UseStaticFiles();
				_ = app.UseResponseCompression();
				_ = app.MapHealthChecks("monke/metrics/health");
				_ = app.MapControllers();
				_ = app.MapFallbackToFile("index.html");

				// Apply database migrations on startup and start the application
				ApplyMigrations(app);
				app.Run();
			}
			catch (Exception ex)
			{
				Log.Fatal(ex, "Application ended with uncaught exception!");
			}
			finally
			{
				Log.CloseAndFlush();
			}
		}

		private static void ApplyMigrations(WebApplication app)
		{
			// Check wether /Data folder exists, if not create it
			if (!Directory.Exists("Data"))
			{
				_ = Directory.CreateDirectory("Data");
			}

			using IServiceScope scope = app.Services.CreateScope();
			MetricsDbContext dbContext = scope.ServiceProvider.GetRequiredService<MetricsDbContext>();
			dbContext.Database.Migrate();
		}
	}
}