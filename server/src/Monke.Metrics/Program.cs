using System.Threading.RateLimiting;

using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;

using Monke.Metrics.Background;
using Monke.Metrics.Data;
using Monke.Metrics.Exceptions;
using Monke.Metrics.Extensions;

namespace Monke.Metrics
{
	public class Program
	{
		public static void Main(string[] args)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
			_ = builder.Services.AddControllers();
			_ = builder.Services.AddHealthChecks();
			_ = builder.Services.AddAnnotatedServices();
			_ = builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
			_ = builder.Services.AddProblemDetails();
			_ = builder.Services.AddHostedService<HardwareInfoService>();
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
			_ = builder.Services.AddResponseCompression(options =>
			{
				options.Providers.Add<BrotliCompressionProvider>();
				options.Providers.Add<GzipCompressionProvider>();
			});
			// ...

			WebApplication app = builder.Build();
			_ = app.UseCors("Network");
			_ = app.UseRateLimiter();
			_ = app.UseExceptionHandler();
			_ = app.UseResponseCompression();
			_ = app.MapHealthChecks("monke/metrics/health");
			_ = app.MapControllers().RequireRateLimiting("fixed");

			// Migrationen anwenden
			using (IServiceScope scope = app.Services.CreateScope())
			{
				MetricsDbContext dbContext = scope.ServiceProvider.GetRequiredService<MetricsDbContext>();
				dbContext.Database.Migrate();
			}

			try
			{
				app.Run();
			}
			catch (Exception)
			{
				_ = Console.ReadKey();
				throw;
			}
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

			_ = builder.Services.AddHttpLogging(options =>
			{
				options.LoggingFields = HttpLoggingFields.RequestPath
									   | HttpLoggingFields.RequestMethod
									   | HttpLoggingFields.ResponseStatusCode
									   | HttpLoggingFields.Duration;
			});
		}

		private static void ConfigureDatabaseContext(WebApplicationBuilder builder)
		{
			_ = builder.Services.AddDbContext<MetricsDbContext>(options =>
				_ = options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
			);
		}
	}
}