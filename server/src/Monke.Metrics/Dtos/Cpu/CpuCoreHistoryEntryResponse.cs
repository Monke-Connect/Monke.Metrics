using Monke.Metrics.Models.Cpu;

namespace Monke.Metrics.Dtos.Cpu
{
	public readonly record struct CpuCoreHistoryEntryResponse
	{
		public ulong PercentProcessorTime { get; init; }
		public DateTimeOffset Timestamp { get; init; }

		public CpuCoreHistoryEntryResponse(CpuCoreHistoryEntry entry)
		{
			this.PercentProcessorTime = entry.PercentProcessorTime;
			this.Timestamp = entry.Timestamp;
		}
	}
}