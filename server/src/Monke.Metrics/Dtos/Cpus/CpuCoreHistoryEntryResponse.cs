using Monke.Metrics.Models.Cpu;

namespace Monke.Metrics.Dtos.Cpus
{
	public sealed record CpuCoreHistoryEntryResponse
	{
		/// <summary>
		/// % Processor Time is the percentage of elapsed time that the processor spends to execute a non-Idle thread. 
		/// </summary>
		public ulong PercentProcessorTime { get; init; }

		/// <summary>
		/// The timestamp when the data was recorded.
		/// </summary>
		public DateTimeOffset Timestamp { get; init; }


		public CpuCoreHistoryEntryResponse()
		{
			this.PercentProcessorTime = 0;
			this.Timestamp = DateTimeOffset.MinValue;
		}


		public CpuCoreHistoryEntryResponse(CpuCoreHistoryEntry entry)
		{
			this.PercentProcessorTime = entry.PercentProcessorTime;
			this.Timestamp = entry.Timestamp;
		}
	}
}