using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Hardware.Info;

namespace Monke.Metrics.Models.Memory
{
	public class MemoryHistoryEntry : BaseModel
	{
		/// <summary>
		/// The amount of physical memory currently available, in bytes. 
		/// </summary>
		[Required]
		public ulong AvailablePhysical { get; set; }

		/// <summary>
		/// The maximum amount of memory the current process can commit, in bytes. 
		/// </summary>
		[Required]
		public ulong AvailablePageFile { get; set; }

		/// <summary>
		/// The amount of unreserved and uncommitted memory currently in the user-mode portion of the virtual address space of the calling process, in bytes.
		/// </summary>
		[Required]
		public ulong AvailableVirtual { get; set; }

		/// <summary>
		/// The time that the memory history entry was recorded.
		/// </summary>
		[Required]
		public DateTimeOffset Timestamp { get; set; }


		public MemoryHistoryEntry()
		{
			this.Id = 0;
			this.AvailablePhysical = 0;
			this.AvailablePageFile = 0;
			this.AvailableVirtual = 0;
			this.Timestamp = DateTimeOffset.MinValue;
		}


		public MemoryHistoryEntry(MemoryStatus status)
		{
			this.Id = 0; // Will be set by EFCore when the entry is added to the database
			this.AvailablePhysical = status.AvailablePhysical;
			this.AvailablePageFile = status.AvailablePageFile;
			this.AvailableVirtual = status.AvailableVirtual;
			this.Timestamp = DateTimeOffset.UtcNow;
		}
	}
}
