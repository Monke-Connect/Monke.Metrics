using Hardware.Info;

using Monke.Metrics.Caches;
using Monke.Metrics.Database;
using Monke.Metrics.Dtos.Drives;
using Monke.Metrics.Exceptions;
using Monke.Metrics.Extensions;

namespace Monke.Metrics.Services
{
	[RegisterService(ServiceLifetime.Scoped, typeof(IDriveService))]
	public class DriveService(ICacheServiceCollection caches, MetricsDbContext dbContext) : IDriveService
	{
		private static readonly TimeSpan MaxHistoryRange = TimeSpan.FromDays(14);
		private readonly ICacheServiceCollection caches = caches ?? throw new ArgumentNullException(nameof(caches));
		private readonly MetricsDbContext dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));


		public List<DriveInfoResponse> GetAllDriveInfos()
		{
			List<DriveInfoResponse> driveList = [];
			(IReadOnlyList<Drive> drives, DateTime lastUpdated) = this.caches.DrivesCache.Get();
			for (int i = 0; i < drives.Count; i++)
			{
				driveList.Add(new DriveInfoResponse(drives[i], lastUpdated));
			}
			return driveList;
		}


		public DriveInfoResponse GetSingleDriveInfo(int driveIndex)
		{
			if (driveIndex < 0)
				throw new BadRequestException($"Requested Drive index {driveIndex} is below zero.");

			(IReadOnlyList<Drive> drives, DateTime lastUpdated) = this.caches.DrivesCache.Get();
			Drive drive = drives.FirstOrDefault(d => d.Index == driveIndex)
				?? throw new NotFoundException($"Requested Drive with index {driveIndex} does not exist.");

			return new DriveInfoResponse(drive, lastUpdated);
		}


		public List<PartitionInfoResponse> GetAllPartitionInfos(int driveIndex)
		{
			if (driveIndex < 0)
				throw new BadRequestException($"Requested Drive index {driveIndex} is below zero.");

			(IReadOnlyList<Drive> drives, DateTime lastUpdated) = this.caches.DrivesCache.Get();
			Drive drive = drives.FirstOrDefault(d => d.Index == driveIndex)
				?? throw new NotFoundException($"Requested Drive with index {driveIndex} does not exist.");

			List<PartitionInfoResponse> partitionList = [];
			for (int i = 0; i < drive.PartitionList.Count; i++)
			{
				partitionList.Add(new PartitionInfoResponse(drive.PartitionList[i], lastUpdated));
			}
			return partitionList;
		}


		public PartitionInfoResponse GetSinglePartitionInfo(int driveIndex, int partitionIndex)
		{
			if (driveIndex < 0)
				throw new BadRequestException($"Requested Drive index {driveIndex} is below zero.");

			if (partitionIndex < 0)
				throw new BadRequestException($"Requested Partition index {partitionIndex} is below zero.");

			(IReadOnlyList<Drive> drives, DateTime lastUpdated) = this.caches.DrivesCache.Get();
			Drive drive = drives.FirstOrDefault(d => d.Index == driveIndex)
				?? throw new NotFoundException($"Requested Drive with index {driveIndex} does not exist.");

			Partition partition = drive.PartitionList.FirstOrDefault(p => p.Index == partitionIndex)
				?? throw new NotFoundException($"Requested Partition with index {partitionIndex} does not exist.");

			return new PartitionInfoResponse(partition, lastUpdated);
		}


		public List<VolumeInfoResponse> GetAllVolumeInfos()
		{
			List<VolumeInfoResponse> volumeList = [];
			(IReadOnlyList<Volume> volumes, DateTime lastUpdated) = this.caches.DrivesCache.GetOrUpdateVolumeCache();
			foreach (Volume v in volumes)
			{
				volumeList.Add(new VolumeInfoResponse(v, lastUpdated));
			}
			return volumeList;
		}


		public VolumeInfoResponse GetSingleVolumeInfo(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
				throw new BadRequestException("The provided volume name can't be empty or whitespace.");

			(IReadOnlyList<Volume> volumes, DateTime lastUpdated) = this.caches.DrivesCache.GetOrUpdateVolumeCache();
			Volume volume = volumes.FirstOrDefault(v => v.Name == name)
				?? throw new NotFoundException($"Requested volume with name {name} does not exist.");

			return new VolumeInfoResponse(volume, lastUpdated);
		}
	}
}
