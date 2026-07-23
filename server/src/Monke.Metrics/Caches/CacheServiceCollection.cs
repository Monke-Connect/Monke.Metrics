using Monke.Metrics.Caches.Cpus;
using Monke.Metrics.Caches.Drives;
using Monke.Metrics.Caches.Memory;
using Monke.Metrics.Extensions;

namespace Monke.Metrics.Caches
{
	[RegisterService(ServiceLifetime.Singleton, typeof(ICacheServiceCollection))]
	public class CacheServiceCollection(
		ICpusCache cpusCache,
		IMemoryCache memoriesCache,
		IMemoryStatusCache memoryStatusCache,
		IDrivesCache drivesCache) : ICacheServiceCollection
	{
		public ICpusCache CpusCache => cpusCache;
		public IMemoryCache MemoryCache => memoriesCache;
		public IMemoryStatusCache MemoryStatusCache => memoryStatusCache;
		public IDrivesCache DrivesCache => drivesCache;
	}
}
