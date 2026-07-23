using System.ComponentModel.DataAnnotations;

using Hardware.Info;

using Microsoft.EntityFrameworkCore;

namespace Monke.Metrics.Models.Cpu
{
	[Index(nameof(CpuIndex), nameof(Timestamp))]
	public class CpuHistoryEntry : BaseModel
	{
		/// <summary>
		/// Index of the CPU within the systems CPUs.
		/// </summary>
		[Required]
		public int CpuIndex { get; init; }

		/// <summary>
		/// Current speed of the processor, in MHz.
		/// </summary>
		[Required]
		public uint CurrentClockSpeed { get; init; }

		/// <summary>
		/// % Processor Time is the percentage of elapsed time that the processor spends to execute a non-Idle thread. 
		/// </summary>
		[Required]
		public ulong PercentProcessorTime { get; set; }

		/// <summary>
		/// The timestamp when the data was recorded.
		/// </summary>
		[Required]
		public DateTimeOffset Timestamp { get; set; }


		public CpuHistoryEntry()
		{
			this.Id = 0;
			this.CpuIndex = 0;
			this.CurrentClockSpeed = 0;
			this.PercentProcessorTime = 0;
			this.Timestamp = DateTimeOffset.UtcNow;
		}


		public CpuHistoryEntry(int cpuIndex, CPU hwCpu)
		{
			this.Id = 0;
			this.CpuIndex = cpuIndex;
			this.CurrentClockSpeed = hwCpu.CurrentClockSpeed;
			this.PercentProcessorTime = hwCpu.PercentProcessorTime;
			this.Timestamp = DateTimeOffset.UtcNow;
		}
	}
}
