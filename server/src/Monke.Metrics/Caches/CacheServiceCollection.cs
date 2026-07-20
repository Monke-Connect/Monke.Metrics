using Monke.Metrics.Caches.Cpu;
using Monke.Metrics.Caches.Memory;
using Monke.Metrics.Extensions;

namespace Monke.Metrics.Caches
{
	[RegisterService(ServiceLifetime.Singleton, typeof(ICacheServiceCollection))]
	public class CacheServiceCollection(
		ICpusCache cpusCache,
		IMemoriesCache memoriesCache,
		IMemoryStatusCache memoryStatusCache) : ICacheServiceCollection
	{
		public ICpusCache CpusCache => cpusCache;
		public IMemoriesCache MemoriesCache => memoriesCache;
		public IMemoryStatusCache MemoryStatusCache => memoryStatusCache;
	}
}
