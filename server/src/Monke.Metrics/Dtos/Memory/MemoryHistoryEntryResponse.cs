using Monke.Metrics.Models.Memory;

namespace Monke.Metrics.Dtos.Memory
{
	public readonly record struct MemoryHistoryEntryResponse
	{
		/// <summary>
		/// The amount of physical memory currently available, in bytes. 
		/// </summary>
		public ulong AvailablePhysical { get; }

		/// <summary>
		/// The maximum amount of memory the current process can commit, in bytes. 
		/// </summary>
		public ulong AvailablePageFile { get; }

		/// <summary>
		/// The amount of unreserved and uncommitted memory currently in the user-mode portion of the virtual address space of the calling process, in bytes.
		/// </summary>
		public ulong AvailableVirtual { get; }

		/// <summary>
		/// The time that the memory history entry was recorded.
		/// </summary>
		public DateTimeOffset Timestamp { get; }


		public MemoryHistoryEntryResponse()
		{
			this.AvailablePhysical = 0;
			this.AvailablePageFile = 0;
			this.AvailableVirtual = 0;
			this.Timestamp = DateTimeOffset.MinValue;
		}


		public MemoryHistoryEntryResponse(MemoryHistoryEntry entry)
		{
			this.AvailablePhysical = entry.AvailablePhysical;
			this.AvailablePageFile = entry.AvailablePageFile;
			this.AvailableVirtual = entry.AvailableVirtual;
			this.Timestamp = entry.Timestamp;
		}
	}
}
