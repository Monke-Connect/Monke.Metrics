using Hardware.Info;

using Monke.Metrics.Extensions;

namespace Monke.Metrics.Caches.Drives
{
	[RegisterService(ServiceLifetime.Singleton, typeof(IDrivesCache))]
	public class DrivesCache : IDrivesCache
	{
		private readonly Lock @lock = new Lock();
		private IReadOnlyList<Drive> value = [];
		private DateTime lastUpdated = DateTime.MinValue;

		/// <inheritdoc/>
		public void Set(IReadOnlyList<Drive> value)
		{
			lock (this.@lock)
			{
				this.value = value ?? [];
				this.lastUpdated = DateTime.UtcNow;
			}
		}

		/// <inheritdoc/>
		public (IReadOnlyList<Drive> Value, DateTime LastUpdated) Get()
		{
			lock (this.@lock)
			{
				return (this.value, this.lastUpdated);
			}
		}
	}
}
