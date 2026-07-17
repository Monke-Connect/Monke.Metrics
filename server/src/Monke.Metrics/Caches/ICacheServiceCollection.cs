using Monke.Metrics.Caches.Implementations;

namespace Monke.Metrics.Caches
{
	public interface ICacheServiceCollection
	{
		ICpusCache CpusCache { get; }
	}
}
