using Hardware.Info;

namespace Monke.Metrics.Caches.Drives
{
	public interface IDrivesCache : ICache<IReadOnlyList<Drive>>
	{
		(IReadOnlyList<Volume> volumes, DateTime lastUpdated) GetOrUpdateVolumeCache();
	}
}
