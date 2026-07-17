using Monke.Metrics.Models.Cpu;

namespace Monke.Metrics.Dtos.Cpu
{
	public readonly record struct CpuHistoryEntryResponse
	{
		public uint CurrentClockSpeed { get; init; }
		public ulong PercentProcessorTime { get; init; }
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
