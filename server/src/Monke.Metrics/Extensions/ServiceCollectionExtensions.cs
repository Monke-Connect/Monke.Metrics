using System.Reflection;
using System.Threading.RateLimiting;

using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;

using Monke.Metrics.Database;

using Serilog;

namespace Monke.Metrics.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddAnnotatedServices(this IServiceCollection services, params Assembly[] assemblies)
		{
			Assembly[] toScan = assemblies.Length > 0 ? assemblies : [Assembly.GetCallingAssembly()];

			foreach (Assembly assembly in toScan)
			{
				IEnumerable<(Type Type, RegisterServiceAttribute? Attr)> candidates = assembly.GetTypes()
					.Where(t => t.IsClass && !t.IsAbstract)
					.Select(t => (Type: t, Attr: t.GetCustomAttribute<RegisterServiceAttribute>()))
					.Where(x => x.Attr is not null);

				foreach ((Type? type, RegisterServiceAttribute? attr) in candidates)
				{
					Type serviceType = attr!.ServiceType ?? type.GetInterfaces().FirstOrDefault() ?? type;
					services.Add(new ServiceDescriptor(serviceType, type, attr.Lifetime));
				}
			}

			return services;
		}

		public static IServiceCollection ConfigureRateLimiting(this IServiceCollection serviceCollection)
		{
			return serviceCollection.AddRateLimiter(options =>
			{
				options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
				{
					return RateLimitPartition.GetFixedWindowLimiter(
						partitionKey: httpContext.Request.Headers.Host.ToString(),
						factory: partition => new FixedWindowRateLimiterOptions
						{
							PermitLimit = 100,
							Window = TimeSpan.FromSeconds(1),
							QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
							QueueLimit = 0
						});
				});
				options.RejectionStatusCode = 429;
			});
		}

		public static IServiceCollection ConfigureDatabaseContext(this IServiceCollection serviceCollection, WebApplicationBuilder builder)
		{
			return serviceCollection.AddDbContext<MetricsDbContext>(options =>
				_ = options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
			);
		}

		public static WebApplicationBuilder ConfigureLogging(this WebApplicationBuilder builder)
		{
			_ = builder.Host.UseSerilog((context, services, config) => config
			   .ReadFrom.Configuration(context.Configuration)
			   .ReadFrom.Services(services)
			   .Enrich.With<ShortSourceContextEnricher>()
			   .Enrich.FromLogContext()
			   .Enrich.WithMachineName()
			   .Enrich.WithThreadId()
			   .WriteTo.Console(outputTemplate:
				   "[{Timestamp:HH:mm:ss} {Level:u3}] {ShortSourceContext}: {Message:lj}{NewLine}{Exception}")
			   .WriteTo.File(
				   path: "Logs/Monke.Metrics..log",
				   rollingInterval: RollingInterval.Day,
				   retainedFileCountLimit: 14,
				   outputTemplate:
					   "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Level:u3}] {ShortSourceContext}: {Message:lj}{NewLine}{Exception}"));
			return builder;

		}

		public static WebApplicationBuilder ConfigureNetworkHandling(this WebApplicationBuilder builder)
		{
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
			return builder;
		}
	}
}
