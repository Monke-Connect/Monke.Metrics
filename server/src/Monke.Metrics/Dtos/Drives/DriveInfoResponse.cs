using Hardware.Info;

namespace Monke.Metrics.Dtos.Drives
{
	public sealed record DriveInfoResponse
	{
		/// <summary>
		/// Physical drive number of the given drive.
		/// </summary>
		public int DriveIndex { get; init; }

		/// <summary>
		/// Label by which the object is known.
		/// </summary>
		public string Name { get; init; }

		/// <summary>
		/// Description of the object.
		/// </summary>
		public string Description { get; init; }

		/// <summary>
		/// Type of media used or accessed by this device.
		/// </summary>
		public string MediaType { get; init; }

		/// <summary>
		/// Name of the disk drive manufacturer.
		/// </summary>
		public string Manufacturer { get; init; }

		/// <summary>
		/// Manufacturer's model number of the disk drive.
		/// </summary>
		public string Model { get; init; }

		/// <summary>
		/// Number allocated by the manufacturer to identify the physical media.
		/// </summary>
		public string SerialNumber { get; init; }

		/// <summary>
		/// Revision for the disk drive firmware that is assigned by the manufacturer.
		/// </summary>
		public string FirmwareRevision { get; init; }

		/// <summary>
		/// Size of the disk drive.
		/// </summary>
		public ulong Size { get; init; }

		/// <summary>
		/// The timestamp when the data was recorded.
		/// </summary>
		public DateTimeOffset LastUpdated { get; init; }


		public DriveInfoResponse()
		{
			this.DriveIndex = 0;
			this.Name = string.Empty;
			this.Description = string.Empty;
			this.MediaType = string.Empty;
			this.Manufacturer = string.Empty;
			this.Model = string.Empty;
			this.SerialNumber = string.Empty;
			this.FirmwareRevision = string.Empty;
			this.Size = 0;
			this.LastUpdated = DateTimeOffset.MinValue;
		}


		public DriveInfoResponse(Drive drive, DateTimeOffset timestamp)
		{
			this.DriveIndex = (int)drive.Index;
			this.Name = drive.Name;
			this.Description = drive.Description;
			this.MediaType = drive.MediaType;
			this.Manufacturer = drive.Manufacturer;
			this.Model = drive.Model;
			this.SerialNumber = drive.SerialNumber;
			this.FirmwareRevision = drive.FirmwareRevision;
			this.Size = drive.Size;
			this.LastUpdated = timestamp;
		}
	}
}
