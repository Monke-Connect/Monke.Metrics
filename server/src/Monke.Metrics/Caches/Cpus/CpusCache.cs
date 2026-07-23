using Hardware.Info;

using Monke.Metrics.Extensions;

namespace Monke.Metrics.Caches.Cpus
{
	[RegisterService(ServiceLifetime.Singleton, typeof(ICpusCache))]
	public class CpusCache : ICpusCache
	{
		private readonly Lock @lock = new Lock();
		private IReadOnlyList<CPU> value = [];
		private DateTime lastUpdated = DateTime.MinValue;

		/// <inheritdoc/>
		public void Set(IReadOnlyList<CPU> value)
		{
			lock (this.@lock)
			{
				this.value = value ?? [];
				this.lastUpdated = DateTime.UtcNow;
			}
		}

		/// <inheritdoc/>
		public (IReadOnlyList<CPU> Value, DateTime LastUpdated) Get()
		{
			lock (this.@lock)
			{
				return (this.value, this.lastUpdated);
			}
		}
	}
}
