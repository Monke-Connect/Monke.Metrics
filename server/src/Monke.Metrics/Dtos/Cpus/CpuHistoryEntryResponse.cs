using Monke.Metrics.Models.Cpu;

namespace Monke.Metrics.Dtos.Cpus
{
	public sealed record CpuHistoryEntryResponse
	{
		/// <summary>
		/// Current speed of the processor, in MHz.
		/// </summary>
		public uint CurrentClockSpeed { get; init; }

		/// <summary>
		/// % Processor Time is the percentage of elapsed time that the processor spends to execute a non-Idle thread. 
		/// </summary>
		public ulong PercentProcessorTime { get; init; }

		/// <summary>
		/// The timestamp when the data was recorded.
		/// </summary>
		public DateTimeOffset Timestamp { get; init; }


		public CpuHistoryEntryResponse()
		{
			this.CurrentClockSpeed = 0;
			this.PercentProcessorTime = 0;
			this.Timestamp = DateTimeOffset.MinValue;
		}


		public CpuHistoryEntryResponse(CpuHistoryEntry entry)
		{
			this.CurrentClockSpeed = entry.CurrentClockSpeed;
			this.PercentProcessorTime = entry.PercentProcessorTime;
			this.Timestamp = entry.Timestamp;
		}
	}
}
