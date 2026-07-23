using System.ComponentModel.DataAnnotations;

using Monke.Metrics.Models.Drives;

namespace Monke.Metrics.Dtos.Drives
{
	public sealed record VolumeHistoryEntryResponse
	{
		/// <summary>
		/// Space, in bytes, available on the logical disk.
		/// </summary>
		public ulong FreeSpace { get; set; }

		/// <summary>
		/// The time that the memory history entry was recorded.
		/// </summary>
		public DateTimeOffset Timestamp { get; set; }


		public VolumeHistoryEntryResponse()
		{
			this.FreeSpace = 0;
			this.Timestamp = DateTimeOffset.MinValue;
		}


		public VolumeHistoryEntryResponse(VolumeHistoryEntry entry)
		{
			ArgumentNullException.ThrowIfNull(entry);

			this.FreeSpace = entry.FreeSpace;
			this.Timestamp = entry.Timestamp;
		}
	}
}
