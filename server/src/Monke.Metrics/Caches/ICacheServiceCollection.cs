using Monke.Metrics.Caches.Cpus;
using Monke.Metrics.Caches.Drives;
using Monke.Metrics.Caches.Memory;

namespace Monke.Metrics.Caches
{
	public interface ICacheServiceCollection
	{
		ICpusCache CpusCache { get; }
		IMemoryCache MemoryCache { get; }
		IMemoryStatusCache MemoryStatusCache { get; }
		IDrivesCache DrivesCache { get; }
	}
}
