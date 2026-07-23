using Hardware.Info;

namespace Monke.Metrics.Dtos.Cpus
{
	public sealed record CpuCoreInfoResponse
	{
		/// <summary>
		/// Index of the Cores CPU within the systems CPUs.
		/// </summary>
		public int CpuIndex { get; init; }

		/// <summary>
		/// Index of the Core within the CPUs cores.
		/// </summary>
		public int CoreIndex { get; init; }

		/// <summary>
		/// Label by which the object is known.
		/// </summary>
		public string Name { get; init; }

		/// <summary>
		/// The timestamp when the data was recorded.
		/// </summary>
		public DateTimeOffset LastUpdated { get; init; }


		public CpuCoreInfoResponse()
		{
			this.CpuIndex = 0;
			this.CoreIndex = 0;
			this.Name = string.Empty;
			this.LastUpdated = DateTimeOffset.MinValue;
		}


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
