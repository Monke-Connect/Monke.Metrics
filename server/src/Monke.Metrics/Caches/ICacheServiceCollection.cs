using Monke.Metrics.Caches.Cpu;
using Monke.Metrics.Caches.Memory;

namespace Monke.Metrics.Caches
{
	public interface ICacheServiceCollection
	{
		ICpusCache CpusCache { get; }
		IMemoriesCache MemoriesCache { get; }
		IMemoryStatusCache MemoryStatusCache { get; }
	}
}
