using System.Threading.RateLimiting;

using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;

using Monke.Metrics.Data;

namespace Monke.Metrics
{
	public class Program
	{
		public static void Main(string[] args)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
			_ = builder.Services.AddControllers();
			_ = builder.Services.AddHealthChecks();
			ConfigureRateLimiting(builder);
			ConfigureLogging(builder);
			ConfigureDatabaseContext(builder);

			_ = builder.Services.AddCors(options =>
				options.AddPolicy("Network", policy =>
				{
					_ = policy
						.AllowAnyOrigin()
						.AllowAnyHeader()
						.AllowAnyMethod();
				})
			);

			WebApplication app = builder.Build();
			_ = app.UseCors("Network");
			_ = app.UseRateLimiter();
			_ = app.MapHealthChecks("monke/metrics/health");
			_ = app.MapControllers().RequireRateLimiting("fixed");

			// Migrationen anwenden
			using (IServiceScope scope = app.Services.CreateScope())
			{
				MetricsDbContext dbContext = scope.ServiceProvider.GetRequiredService<MetricsDbContext>();
				dbContext.Database.Migrate();
			}

			app.Run();

		}


		private static void ConfigureRateLimiting(WebApplicationBuilder builder)
		{
			_ = builder.Services.AddRateLimiter(options =>
			{
				_ = options.AddFixedWindowLimiter("fixed", opt =>
				{
					opt.PermitLimit = 4;
					opt.Window = TimeSpan.FromSeconds(12);
					opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
					opt.QueueLimit = 2;
				});
			});
		}

		private static void ConfigureLogging(WebApplicationBuilder builder)
		{
			// Logging
			_ = builder.Logging.ClearProviders();
			_ = builder.Logging.AddConsole();
		}

		private static void ConfigureDatabaseContext(WebApplicationBuilder builder)
		{
			_ = builder.Services.AddDbContext<MetricsDbContext>(options =>
				_ = options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
			);
		}
	}
}