using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Hardware.Info;

namespace Monke.Metrics.Models.Cpu
{
	public class CpuCoreHistoryEntry
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; init; }

		[Required]
		public int CpuIndex { get; init; } 

		[Required]
		public int CoreIndex { get; init; } 

		[Required]
		public ulong PercentProcessorTime { get; init; } 

		[Required]
		public DateTimeOffset Timestamp { get; init; }


		public CpuCoreHistoryEntry()
		{
			this.CpuIndex = 0;
			this.CoreIndex = 0;
			this.PercentProcessorTime = 0;
			this.Timestamp = DateTimeOffset.UtcNow;
		}

		public CpuCoreHistoryEntry(int cpuIndex, int coreIndex, CpuCore hwCpuCore)
		{
			this.CpuIndex = cpuIndex;
			this.CoreIndex = coreIndex;
			this.PercentProcessorTime = hwCpuCore.PercentProcessorTime;
			this.Timestamp = DateTimeOffset.UtcNow;
		}
	}
}
