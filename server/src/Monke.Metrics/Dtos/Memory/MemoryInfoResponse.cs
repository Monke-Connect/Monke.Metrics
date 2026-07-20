using Hardware.Info;

namespace Monke.Metrics.Dtos.Memory
{
	public readonly record struct MemoryInfoResponse
	{
		public int Index { get; init; }

		/// <summary> 
		/// Physically labeled bank where the memory is located. 
		/// </summary>
		public string BankLabel { get; init; } = string.Empty;

		/// <summary> Total capacity of the physical memory in bytes.
		/// </summary>
		public ulong Capacity { get; init; }

		/// <summary>
		/// Data width of the physical memory in bits.
		/// </summary>
		public ushort DataWidth { get; init; }

		/// <summary>
		/// Implementation form factor for the chip.
		/// </summary>
		public FormFactor FormFactor { get; init; }

		/// <summary>
		/// Name of the organization responsible for producing the physical element.
		/// </summary>
		public string Manufacturer { get; init; } = string.Empty;

		/// <summary>
		/// The maximum operating voltage for this device, in millivolts, or 0, if the voltage is unknown.
		/// </summary>
		public uint MaxVoltage { get; init; }

		/// <summary>
		/// The minimum operating voltage for this device, in millivolts, or 0, if the voltage is unknown.
		/// </summary>
		public uint MinVoltage { get; init; }

		/// <summary>
		/// Part number assigned by the organization responsible for producing or manufacturing the physical element.
		/// </summary>
		public string PartNumber { get; init; } = string.Empty;

		/// <summary>
		/// Manufacturer-allocated number to identify the physical element.
		/// </summary>
		public string SerialNumber { get; init; } = string.Empty;

		/// <summary>
		/// Type of physical memory.
		/// </summary>
		public MemoryType MemoryType { get; init; }

		/// <summary>
		/// Speed of the physical memory in nanoseconds.
		/// </summary>
		public uint Speed { get; init; }

		public DateTimeOffset Timestamp { get; init; }

		public MemoryInfoResponse()
		{
			this.Index = 0;
			this.BankLabel = string.Empty;
			this.Capacity = 0;
			this.DataWidth = 0;
			this.FormFactor = FormFactor.UNKNOWN;
			this.Manufacturer = string.Empty;
			this.MaxVoltage = 0;
			this.MinVoltage = 0;
			this.PartNumber = string.Empty;
			this.SerialNumber = string.Empty;
			this.MemoryType = MemoryType.Unknown;
			this.Speed = 0;
			this.Timestamp = DateTimeOffset.MinValue;
		}

		public MemoryInfoResponse(int index, Hardware.Info.Memory memory, DateTimeOffset timestamp)
		{
			this.Index = index;
			this.BankLabel = memory.BankLabel;
			this.Capacity = memory.Capacity;
			this.DataWidth = memory.DataWidth;
			this.FormFactor = memory.FormFactor;
			this.Manufacturer = memory.Manufacturer;
			this.MaxVoltage = memory.MaxVoltage;
			this.MinVoltage = memory.MinVoltage;
			this.PartNumber = memory.PartNumber;
			this.SerialNumber = memory.SerialNumber;
			this.MemoryType = memory.MemoryType;
			this.Speed = memory.Speed;
			this.Timestamp = timestamp;
		}
	}
}
