using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Hardware.Info;

namespace Monke.Metrics.Models.Cpu
{
	public class CpuHistoryEntry
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; init; }

		[Required]
		public int CpuIndex { get; init; }

		[Required]
		public uint CurrentClockSpeed { get; init; }

		[Required]
		public ulong PercentProcessorTime { get; set; }

		[Required]
		public DateTimeOffset Timestamp { get; set; }


		public CpuHistoryEntry()
		{
			this.CpuIndex = 0;
			this.CurrentClockSpeed = 0;
			this.PercentProcessorTime = 0;
			this.Timestamp = DateTimeOffset.UtcNow;
		}


		public CpuHistoryEntry(int cpuIndex, CPU hwCpu)
		{
			this.CpuIndex = cpuIndex;
			this.CurrentClockSpeed = hwCpu.CurrentClockSpeed;
			this.PercentProcessorTime = hwCpu.PercentProcessorTime;
			this.Timestamp = DateTimeOffset.UtcNow;
		}
	}
}
