using Hardware.Info;

namespace Monke.Metrics.Dtos.Cpu
{
	public readonly record struct CpuInfoResponse
	{
		public int CpuIndex { get; init; }
		public string Name { get; init; }
		public string Caption { get; init; }
		public string ProcessorId { get; init; }
		public string Description { get; init; }
		public uint L1InstructionCacheSize { get; init; }
		public uint L1DataCacheSize { get; init; }
		public uint L2CacheSize { get; init; }
		public uint L3CacheSize { get; init; }
		public string Manufacturer { get; init; }
		public uint MaxClockSpeed { get; init; }
		public uint NumberOfCores { get; init; }
		public uint NumberOfLogicalProcessors { get; init; }
		public bool SecondLevelAddressTranslationExtensions { get; init; }
		public string SocketDesignation { get; init; }
		public bool VirtualizationFirmwareEnabled { get; init; }
		public bool VMMonitorModeExtensions { get; init; }
		public DateTimeOffset LastUpdated { get; init; }

		public CpuInfoResponse(int cpuIndex, CPU hwCpu, DateTime lastUpdated)
		{
			this.CpuIndex = cpuIndex;
			this.Name = hwCpu.Name;
			this.Caption = hwCpu.Caption;
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
			this.SecondLevelAddressTranslationExtensions = hwCpu.SecondLevelAddressTranslationExtensions;
			this.SocketDesignation = hwCpu.SocketDesignation;
			this.VirtualizationFirmwareEnabled = hwCpu.VirtualizationFirmwareEnabled;
			this.VMMonitorModeExtensions = hwCpu.VMMonitorModeExtensions;
			this.LastUpdated = lastUpdated;
		}
	}
}