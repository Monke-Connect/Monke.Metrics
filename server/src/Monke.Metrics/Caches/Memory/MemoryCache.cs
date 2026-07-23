using Monke.Metrics.Extensions;

namespace Monke.Metrics.Caches.Memory
{
	[RegisterService(ServiceLifetime.Singleton, typeof(IMemoryCache))]
	public class MemoryCache : IMemoryCache
	{
		private readonly Lock @lock = new Lock();
		private IReadOnlyList<Hardware.Info.Memory> value = [];
		private DateTime lastUpdated = DateTime.MinValue;

		/// <inheritdoc/>
		public void Set(IReadOnlyList<Hardware.Info.Memory> value)
		{
			lock (this.@lock)
			{
				this.value = value ?? [];
				this.lastUpdated = DateTime.UtcNow;
			}
		}

		/// <inheritdoc/>
		public (IReadOnlyList<Hardware.Info.Memory> Value, DateTime LastUpdated) Get()
		{
			lock (this.@lock)
			{
				return (this.value, this.lastUpdated);
			}
		}
	}
}
