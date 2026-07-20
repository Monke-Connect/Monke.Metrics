using Hardware.Info;

using Monke.Metrics.Extensions;

namespace Monke.Metrics.Caches.Memory
{
	[RegisterService(ServiceLifetime.Singleton, typeof(IMemoryStatusCache))]
	public class MemoryStatusCache : IMemoryStatusCache
	{
		private readonly Lock @lock = new Lock();
		private MemoryStatus value = new MemoryStatus();
		private DateTime lastUpdated = DateTime.MinValue;

		/// <inheritdoc/>
		public void Set(MemoryStatus value)
		{
			lock (this.@lock)
			{
				this.value = value;
				this.lastUpdated = DateTime.UtcNow;
			}
		}

		/// <inheritdoc/>
		public (MemoryStatus Value, DateTime LastUpdated) Get()
		{
			lock (this.@lock)
			{
				return (this.value, this.lastUpdated);
			}
		}
	}
}
