using Hardware.Info;

using Monke.Metrics.Extensions;

namespace Monke.Metrics.Caches.Drives
{
	[RegisterService(ServiceLifetime.Singleton, typeof(IDrivesCache))]
	public class DrivesCache : IDrivesCache
	{
		private readonly Lock @lock = new Lock();
		private IReadOnlyList<Drive> drives = [];
		private IReadOnlyList<Volume> volumes = [];
		private DateTime lastUpdated = DateTime.MinValue;
		private bool AreVolumesPresent;

		/// <inheritdoc/>
		public void Set(IReadOnlyList<Drive> value)
		{
			lock (this.@lock)
			{
				this.drives = value ?? [];
				this.lastUpdated = DateTime.UtcNow;
				this.InvalidateVolumes();
			}
		}

		/// <inheritdoc/>
		public (IReadOnlyList<Drive> Value, DateTime LastUpdated) Get()
		{
			lock (this.@lock)
			{
				return (this.drives, this.lastUpdated);
			}
		}

		private void InvalidateVolumes()
		{
			this.volumes = [];
			this.AreVolumesPresent = false;
		}

		private void SetCachedVolumes(List<Volume> volumes)
		{
			lock (this.@lock)
			{
				this.volumes = volumes.AsReadOnly();
				this.AreVolumesPresent = true;
			}
		}

		private (IReadOnlyList<Volume> Value, DateTime LastUpdated) GetCachedVolumes()
		{
			lock (this.@lock)
			{
				return (this.volumes, this.lastUpdated);
			}
		}

		private static List<Volume> GetUniqueVolumes(IReadOnlyList<Drive> drives)
		{
			Dictionary<string, Volume> uniqueVolumes = [];
			foreach (Drive drive in drives)
			{
				foreach (Partition partition in drive.PartitionList)
				{
					foreach (Volume volume in partition.VolumeList)
					{
						_ = uniqueVolumes.TryAdd(volume.Name, volume);
					}
				}
			}
			return [.. uniqueVolumes.Values];
		}

		public (IReadOnlyList<Volume> volumes, DateTime lastUpdated) GetOrUpdateVolumeCache()
		{
			IReadOnlyList<Volume> volumes;
			DateTime lastUpdated;

			// Get volumes from cache or set cache if not present
			if (this.AreVolumesPresent)
			{
				(volumes, lastUpdated) = this.GetCachedVolumes();
			}
			else
			{
				(IReadOnlyList<Drive> drives, lastUpdated) = this.Get();
				List<Volume> uniques = GetUniqueVolumes(drives);
				this.SetCachedVolumes(uniques);
				volumes = uniques.AsReadOnly();
			}
			return (volumes, lastUpdated);
		}
	}
}
