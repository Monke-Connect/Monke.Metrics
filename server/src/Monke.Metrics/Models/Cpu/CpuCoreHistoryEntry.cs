using System.ComponentModel.DataAnnotations;

using Hardware.Info;

using Microsoft.EntityFrameworkCore;

namespace Monke.Metrics.Models.Cpu
{
	[Index(nameof(CpuIndex), nameof(CoreIndex), nameof(Timestamp))]
	public class CpuCoreHistoryEntry : BaseModel
	{
		/// <summary>
		/// Index of the Cores CPU within the systems CPUs.
		/// </summary>
		[Required]
		public int CpuIndex { get; init; }

		/// <summary>
		/// Index of the Core within the CPUs cores.
		/// </summary>
		[Required]
		public int CoreIndex { get; init; }

		/// <summary>
		/// % Processor Time is the percentage of elapsed time that the processor spends to execute a non-Idle thread. 
		/// </summary>
		[Required]
		public ulong PercentProcessorTime { get; init; }

		/// <summary>
		/// The timestamp when the data was recorded.
		/// </summary>
		[Required]
		public DateTimeOffset Timestamp { get; init; }


		public CpuCoreHistoryEntry()
		{
			this.Id = 0;
			this.CpuIndex = 0;
			this.CoreIndex = 0;
			this.PercentProcessorTime = 0;
			this.Timestamp = DateTimeOffset.UtcNow;
		}


		public CpuCoreHistoryEntry(int cpuIndex, int coreIndex, CpuCore hwCpuCore)
		{
			this.Id = 0;
			this.CpuIndex = cpuIndex;
			this.CoreIndex = coreIndex;
			this.PercentProcessorTime = hwCpuCore.PercentProcessorTime;
			this.Timestamp = DateTimeOffset.UtcNow;
		}
	}
}
