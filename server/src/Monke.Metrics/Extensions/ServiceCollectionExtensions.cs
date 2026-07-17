using System.Reflection;

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
	}
}
