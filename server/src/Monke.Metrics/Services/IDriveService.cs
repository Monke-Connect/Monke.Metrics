using Monke.Metrics.Dtos.Drives;

namespace Monke.Metrics.Services
{
	public interface IDriveService
	{
		List<DriveInfoResponse> GetAllDriveInfos();
		List<PartitionInfoResponse> GetAllPartitionInfos(int driveIndex);
		DriveInfoResponse GetSingleDriveInfo(int driveIndex);
		PartitionInfoResponse GetSinglePartitionInfo(int driveIndex, int partitionIndex);
	}
}
