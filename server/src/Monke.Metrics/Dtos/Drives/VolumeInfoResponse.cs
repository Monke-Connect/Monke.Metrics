using Hardware.Info;

namespace Monke.Metrics.Dtos.Drives
{
	public sealed record VolumeInfoResponse
	{
		/// <summary>
		/// Label by which the object is known.
		/// </summary>
		public string Name { get; init; }

		/// <summary>
		/// Description of the object.
		/// </summary>
		public string Description { get; init; }

		/// <summary>
		/// Volume serial number of the logical disk.
		/// </summary>
		public string SerialNumber { get; init; }

		/// <summary>
		/// File system on the logical disk.
		/// </summary>
		public string FileSystem { get; init; }

		/// <summary>
		/// If True, the logical volume exists as a single compressed entity, such as a DoubleSpace volume. 
		/// </summary>
		public bool IsCompressed { get; init; }

		/// <summary>
		/// Space, in bytes, available on the logical disk.
		/// </summary>
		public ulong FreeSpace { get; init; }

		/// <summary>
		/// Size of the disk drive.
		/// </summary>
		public ulong Size { get; init; }

		/// <summary>
		/// The timestamp when the data was recorded.
		/// </summary>
		public DateTimeOffset LastUpdated { get; init; }


		public VolumeInfoResponse()
		{
			this.Name = string.Empty;
			this.Description = string.Empty;
			this.SerialNumber = string.Empty;
			this.FileSystem = string.Empty;
			this.IsCompressed = false;
			this.FreeSpace = 0;
			this.Size = 0;
			this.LastUpdated = DateTimeOffset.MinValue;
		}


		public VolumeInfoResponse(int diskIndex, int partitionIndex, Volume volume, DateTimeOffset timestamp)
		{
			this.Name = volume.Name;
			this.Description = volume.Description;
			this.SerialNumber = volume.VolumeSerialNumber;
			this.FileSystem = volume.FileSystem;
			this.IsCompressed = volume.Compressed;
			this.FreeSpace = volume.FreeSpace;
			this.Size = volume.Size;
			this.LastUpdated = timestamp;
		}
	}
}
