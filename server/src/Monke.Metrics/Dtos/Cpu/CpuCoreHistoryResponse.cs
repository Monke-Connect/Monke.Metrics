using Monke.Metrics.Models.Cpu;

namespace Monke.Metrics.Dtos.Cpu
{
	public readonly record struct CpuCoreHistoryResponse
	{
		public int CpuIndex { get; init; }
		public int CoreIndex { get; init; }
		public DateTimeOffset StartDateTime { get; init; }
		public DateTimeOffset EndDateTime { get; init; }
		public List<CpuCoreHistoryEntryResponse> Entries { get; init; }

		public CpuCoreHistoryResponse()
		{
			this.CpuIndex = 0;
			this.StartDateTime = DateTimeOffset.MinValue;
			this.EndDateTime = DateTimeOffset.MinValue;
			this.Entries = [];
		}

		public CpuCoreHistoryResponse(int cpuIndex, int coreIndex, DateTimeOffset startDateTime, DateTimeOffset endDateTime, List<CpuCoreHistoryEntry> entries)
		{
			this.CpuIndex = cpuIndex;
			this.StartDateTime = startDateTime;
			this.EndDateTime = endDateTime;
			this.Entries = [.. entries.Select(v => new CpuCoreHistoryEntryResponse(v))];
		}
	}
}
