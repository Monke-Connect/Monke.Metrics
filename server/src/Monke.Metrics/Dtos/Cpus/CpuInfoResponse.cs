using Hardware.Info;

namespace Monke.Metrics.Dtos.Cpus
{
	public sealed record CpuInfoResponse
	{
		/// <summary>
		/// Index of the CPU within the systems CPUs.
		/// </summary>
		public int CpuIndex { get; init; }

		/// <summary>
		/// Label by which the object is known.
		/// </summary>
		public string Name { get; init; }

		/// <summary>
		/// Processor information that describes the processor features. 
		/// </summary>
		public string ProcessorId { get; init; }

		/// <summary>
		/// Description of the object.
		/// </summary>
		public string Description { get; init; }

		/// <summary>
		/// Size of the Level 1 processor instruction cache.
		/// </summary>
		public uint L1InstructionCacheSize { get; init; }

		/// <summary>
		/// Size of the Level 1 processor data cache.
		/// </summary>
		public uint L1DataCacheSize { get; init; }

		/// <summary>
		/// Size of the Level 2 processor cache. 
		/// </summary>
		public uint L2CacheSize { get; init; }

		/// <summary>
		/// Size of the Level 3 processor cache.
		/// </summary>
		public uint L3CacheSize { get; init; }

		/// <summary>
		/// Name of the processor manufacturer.
		/// </summary>
		public string Manufacturer { get; init; }

		/// <summary>
		/// Maximum speed of the processor, in MHz.
		/// </summary>
		public uint MaxClockSpeed { get; init; }

		/// <summary>
		/// Number of cores for the current instance of the processor. A core is a physical processor on the integrated circuit.
		/// </summary>
		public uint NumberOfCores { get; init; }

		/// <summary>
		/// Number of logical processors for the current instance of the processor.
		/// </summary>
		public uint NumberOfLogicalProcessors { get; init; }

		/// <summary>
		/// If True, the processor supports address translation extensions used for virtualization.
		/// </summary>
		public bool HasSecondLevelAddressTranslationExtensions { get; init; }

		/// <summary>
		/// Type of chip socket used on the circuit.
		/// </summary>
		public string SocketDesignation { get; init; }

		/// <summary>
		/// If True, the Firmware has enabled virtualization extensions.
		/// </summary>
		public bool IsVirtualizationFirmwareEnabled { get; init; }

		/// <summary>
		/// If True, the processor supports Intel or AMD Virtual Machine Monitor extensions.
		/// </summary>
		public bool HasVMMonitorModeExtensions { get; init; }

		/// <summary>
		/// The timestamp when the data was recorded.
		/// </summary>
		public DateTimeOffset LastUpdated { get; init; }


		public CpuInfoResponse()
		{
			this.CpuIndex = 0;
			this.Name = string.Empty;
			this.ProcessorId = string.Empty;
			this.Description = string.Empty;
			this.L1InstructionCacheSize = 0;
			this.L1DataCacheSize = 0;
			this.L2CacheSize = 0;
			this.L3CacheSize = 0;
			this.Manufacturer = string.Empty;
			this.MaxClockSpeed = 0;
			this.NumberOfCores = 0;
			this.NumberOfLogicalProcessors = 0;
			this.HasSecondLevelAddressTranslationExtensions = false;
			this.SocketDesignation = string.Empty;
			this.IsVirtualizationFirmwareEnabled = false;
			this.HasVMMonitorModeExtensions = false;
			this.LastUpdated = DateTimeOffset.MinValue;
		}


		public CpuInfoResponse(int cpuIndex, CPU hwCpu, DateTime lastUpdated)
		{
			this.CpuIndex = cpuIndex;
			this.Name = hwCpu.Name;
			this.ProcessorId = hwCpu.ProcessorId;
			this.Description = hwCpu.Description;
			this.L1InstructionCacheSize = hwCpu.L1InstructionCacheSize;
			this.L1DataCacheSize = hwCpu.L1DataCacheSize;
			this.L2CacheSize = hwCpu.L2CacheSize;
			this.L3CacheSize = hwCpu.L3CacheSize;
			this.Manufacturer = hwCpu.Manufacturer;
			this.MaxClockSpeed = hwCpu.MaxClockSpeed;
			this.NumberOfCores = hwCpu.NumberOfCores;
			this.NumberOfLogicalProcessors = hwCpu.NumberOfLogicalProcessors;
			this.HasSecondLevelAddressTranslationExtensions = hwCpu.SecondLevelAddressTranslationExtensions;
			this.SocketDesignation = hwCpu.SocketDesignation;
			this.IsVirtualizationFirmwareEnabled = hwCpu.VirtualizationFirmwareEnabled;
			this.HasVMMonitorModeExtensions = hwCpu.VMMonitorModeExtensions;
			this.LastUpdated = lastUpdated;
		}
	}
}