using Hardware.Info;

namespace Monke.Metrics.Dtos.Drives
{
	public sealed record PartitionInfoResponse
	{
		/// <summary>
		/// Index number of the disk containing this partition.
		/// </summary>
		public int DriveIndex { get; init; }

		/// <summary>
		/// Index number of the partition.
		/// </summary>
		public int PartitionIndex { get; init; }

		/// <summary>
		/// Label by which the object is known.
		/// </summary>
		public string Name { get; init; }

		/// <summary>
		/// Description of the object.
		/// </summary>
		public string Description { get; init; }

		/// <summary>
		/// Indicates whether the computer can be booted from this partition.
		/// </summary>
		public bool IsBootable { get; init; }

		/// <summary>
		/// Partition is the active partition. 
		/// </summary>
		public bool IsBootPartition { get; init; }

		/// <summary>
		/// If True, this is the primary partition.
		/// </summary>
		public bool IsPrimaryPartition { get; init; }

		/// <summary>
		/// Total size of the partition.
		/// </summary>
		public ulong Size { get; init; }

		/// <summary>
		/// Starting offset (in bytes) of the partition.
		/// </summary>
		public ulong StartingOffset { get; init; }

		/// <summary>
		/// The timestamp when the data was recorded.
		/// </summary>
		public DateTimeOffset LastUpdated { get; init; }


		public PartitionInfoResponse()
		{
			this.DriveIndex = 0;
			this.PartitionIndex = 0;
			this.Name = string.Empty;
			this.Description = string.Empty;
			this.IsBootable = false;
			this.IsBootPartition = false;
			this.IsPrimaryPartition = false;
			this.Size = 0;
			this.StartingOffset = 0;
			this.LastUpdated = DateTimeOffset.MinValue;
		}

		public PartitionInfoResponse(Partition partition, DateTimeOffset timestamp)
		{
			this.DriveIndex = (int)partition.DiskIndex;
			this.PartitionIndex = (int)partition.Index;
			this.Name = partition.Name;
			this.Description = partition.Description;
			this.IsBootable = partition.Bootable;
			this.IsBootPartition = partition.BootPartition;
			this.IsPrimaryPartition = partition.PrimaryPartition;
			this.Size = partition.Size;
			this.StartingOffset = partition.StartingOffset;
			this.LastUpdated = timestamp;
		}
	}
}
