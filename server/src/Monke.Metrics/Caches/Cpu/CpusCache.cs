using Hardware.Info;

using Monke.Metrics.Caches.Implementations;
using Monke.Metrics.Extensions;
using Monke.Metrics.Services;

namespace Monke.Metrics.Caches.Cpu
{
	[RegisterService(ServiceLifetime.Singleton, typeof(ICpusCache))]
	public class CpusCache : ICpusCache
	{
		private readonly Lock @lock = new Lock();
		private IReadOnlyList<CPU> value = [];
		private DateTime lastUpdated = DateTime.MinValue;

		/// <inheritdoc/>
		public void Set(IReadOnlyList<CPU> cpus)
		{
			lock (this.@lock)
			{
				this.value = cpus ?? [];
				this.lastUpdated = DateTime.UtcNow;
			}
		}

		/// <inheritdoc/>
		public (IReadOnlyList<CPU> Cpus, DateTime LastUpdated) Get()
		{
			lock (this.@lock)
			{
				return (this.value, this.lastUpdated);
			}
		}
	}
}
