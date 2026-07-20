using Hardware.Info;

using Monke.Metrics.Models.Memory;

namespace Monke.Metrics.Dtos.Memory
{
	public readonly record struct MemoryHistoryResponse
	{
		public DateTimeOffset StartDateTime { get; init; }
		public DateTimeOffset EndDateTime { get; init; }
		public ulong TotalPhysical { get; init; }
		public ulong TotalPageFile { get; init; }
		public ulong TotalVirtual { get; init; }
		public List<MemoryHistoryEntryResponse> Entries { get; init; }

		public MemoryHistoryResponse()
		{
			this.StartDateTime = DateTimeOffset.MinValue;
			this.EndDateTime = DateTimeOffset.MinValue;
			this.TotalVirtual = 0;
			this.TotalPageFile = 0;
			this.TotalPhysical = 0;
			this.Entries = [];
		}

		public MemoryHistoryResponse(DateTimeOffset startDateTime, DateTimeOffset endDateTime, MemoryStatus status, List<MemoryHistoryEntry> entries)
		{
			this.StartDateTime = startDateTime;
			this.EndDateTime = endDateTime;
			this.TotalVirtual = status.TotalVirtual;
			this.TotalPageFile = status.TotalPageFile;
			this.TotalPhysical = status.TotalPhysical;
			this.Entries = [.. entries.Select(v => new MemoryHistoryEntryResponse(v))];
		}
	}
}
