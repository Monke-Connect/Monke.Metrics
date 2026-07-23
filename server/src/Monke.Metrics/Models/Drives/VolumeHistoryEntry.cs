using System.ComponentModel.DataAnnotations;

using Hardware.Info;

using Microsoft.EntityFrameworkCore;

namespace Monke.Metrics.Models.Drives
{
	[Index(nameof(Name), nameof(Timestamp))]
	public class VolumeHistoryEntry : BaseModel
	{
		/// <summary>
		/// Volume name of the logical disk.
		/// </summary>
		[Required]
		public string Name { get; set; } = string.Empty;

		/// <summary>
		/// Space, in bytes, available on the logical disk.
		/// </summary>
		[Required]
		public ulong FreeSpace { get; set; }

		/// <summary>
		/// The time that the memory history entry was recorded.
		/// </summary>
		[Required]
		public DateTimeOffset Timestamp { get; set; }


		public VolumeHistoryEntry()
		{
			this.Id = 0;
			this.Name = string.Empty;
			this.FreeSpace = 0;
			this.Timestamp = DateTimeOffset.MinValue;
		}

		public VolumeHistoryEntry(Volume volume)
		{
			this.Id = 0;
			this.Name = volume.Name;
			this.FreeSpace = volume.FreeSpace;
			this.Timestamp = DateTimeOffset.UtcNow;
		}
	}
}
