using Monke.Metrics.Models.Cpu;

namespace Monke.Metrics.Dtos.Cpu
{
	public readonly record struct CpuHistoryResponse
	{
		public int CpuIndex { get; init; }
		public DateTimeOffset StartDateTime { get; init; }
		public DateTimeOffset EndDateTime { get; init; }
		public List<CpuHistoryEntryResponse> Entries { get; init; }

		public CpuHistoryResponse()
		{
			this.CpuIndex = 0;
			this.StartDateTime = DateTimeOffset.MinValue;
			this.EndDateTime = DateTimeOffset.MinValue;
			this.Entries = [];
		}

		public CpuHistoryResponse(int cpuIndex, DateTimeOffset startDateTime, DateTimeOffset endDateTime, List<CpuHistoryEntry> entries)
		{
			this.CpuIndex = cpuIndex;
			this.StartDateTime = startDateTime;
			this.EndDateTime = endDateTime;
			this.Entries = [.. entries.Select(v => new CpuHistoryEntryResponse(v))];
		}
	}
}
