using Hardware.Info;

namespace Monke.Metrics.Dtos.Cpu
{
	public readonly record struct CpuCoreInfoResponse
	{
		public int CpuIndex { get; init; }
		public int CoreIndex { get; init; }
		public string Name { get; init; }
		public DateTimeOffset LastUpdated { get; init; }

		public CpuCoreInfoResponse(int cpuIndex, int coreIndex, CpuCore core, DateTimeOffset lastUpdated)
		{
			ArgumentOutOfRangeException.ThrowIfNegative(cpuIndex);
			ArgumentOutOfRangeException.ThrowIfNegative(coreIndex);
			ArgumentNullException.ThrowIfNull(core);

			this.CpuIndex = cpuIndex;
			this.CoreIndex = coreIndex;
			this.Name = core.Name;
			this.LastUpdated = lastUpdated;
		}
	}
}
