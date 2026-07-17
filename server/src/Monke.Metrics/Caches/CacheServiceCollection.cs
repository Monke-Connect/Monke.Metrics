using Monke.Metrics.Caches.Implementations;
using Monke.Metrics.Extensions;

namespace Monke.Metrics.Caches
{
	[RegisterService(ServiceLifetime.Singleton, typeof(ICacheServiceCollection))]
	public class CacheServiceCollection(ICpusCache cpusCache) : ICacheServiceCollection
	{
		public ICpusCache CpusCache => cpusCache;
	}
}
