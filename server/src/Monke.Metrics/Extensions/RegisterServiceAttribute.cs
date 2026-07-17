namespace Monke.Metrics.Extensions
{
	[AttributeUsage(AttributeTargets.Class)]
	public class RegisterServiceAttribute(ServiceLifetime lifetime = ServiceLifetime.Scoped, Type? serviceType = null) : Attribute
	{
		public ServiceLifetime Lifetime { get; } = lifetime;
		public Type? ServiceType { get; } = serviceType;
	}
}
