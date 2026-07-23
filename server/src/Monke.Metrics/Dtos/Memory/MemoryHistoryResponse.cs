using Hardware.Info;

using Monke.Metrics.Models.Memory;

namespace Monke.Metrics.Dtos.Memory
{
	public readonly record struct MemoryHistoryResponse
	{
		/// <summary>
		/// The amount of actual physical memory, in bytes.
		/// </summary>
		public ulong TotalPhysical { get; init; }

		/// <summary>
		/// The current committed memory limit for the system or the current process, whichever is smaller, in bytes.
		/// </summary>
		public ulong TotalPageFile { get; init; }

		/// <summary>
		/// The size of the user-mode portion of the virtual address space of the calling process, in bytes. 
		/// </summary>
		public ulong TotalVirtual { get; init; }

		/// <summary>
		/// The starting datetime of the history entries.
		/// </summary>
		public DateTimeOffset StartDateTime { get; init; }

		/// <summary>
		/// The ending datetime of the history entries.
		/// </summary>
		public DateTimeOffset EndDateTime { get; init; }

		/// <summary>
		/// Collection of history entries, that describe the metric history of the system.
		/// </summary>
		public List<MemoryHistoryEntryResponse> Entries { get; init; }


		public MemoryHistoryResponse()
		{
			this.TotalVirtual = 0;
			this.TotalPageFile = 0;
			this.TotalPhysical = 0;
			this.StartDateTime = DateTimeOffset.MinValue;
			this.EndDateTime = DateTimeOffset.MinValue;
			this.Entries = [];
		}


		public MemoryHistoryResponse(DateTimeOffset startDateTime, DateTimeOffset endDateTime, MemoryStatus status, List<MemoryHistoryEntry> entries)
		{
			this.TotalVirtual = status.TotalVirtual;
			this.TotalPageFile = status.TotalPageFile;
			this.TotalPhysical = status.TotalPhysical;
			this.StartDateTime = startDateTime;
			this.EndDateTime = endDateTime;
			this.Entries = [.. entries.Select(v => new MemoryHistoryEntryResponse(v))];
		}
	}
}
