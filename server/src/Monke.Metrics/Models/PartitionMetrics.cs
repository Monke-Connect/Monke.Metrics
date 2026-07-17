using Hardware.Info;

namespace Monke.Metrics.Models
{
	public class PartitionMetrics
	{
		public PartitionMetrics(Partition partition)
		{
			ArgumentNullException.ThrowIfNull(partition);
		}

		public static List<PartitionMetrics> FromHardwareInfo(HardwareInfo hardwareInfo)
		{
			ArgumentNullException.ThrowIfNull(hardwareInfo);
			var partitions = new List<PartitionMetrics>();
			return partitions;
		}
	}
}
