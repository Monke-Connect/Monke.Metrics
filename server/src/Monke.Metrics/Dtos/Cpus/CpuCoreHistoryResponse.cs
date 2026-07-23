using Monke.Metrics.Models.Cpu;

namespace Monke.Metrics.Dtos.Cpus
{
	public sealed record CpuCoreHistoryResponse
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
		/// The starting datetime of the history entries.
		/// </summary>
		public DateTimeOffset StartDateTime { get; init; }

		/// <summary>
		/// The ending datetime of the history entries.
		/// </summary>
		public DateTimeOffset EndDateTime { get; init; }

		/// <summary>
		/// Collection of history entries, that describe the metric history of the system.
		/// </summary>
		public List<CpuCoreHistoryEntryResponse> Entries { get; init; }


		public CpuCoreHistoryResponse()
		{
			this.CpuIndex = 0;
			this.CoreIndex = 0;
			this.StartDateTime = DateTimeOffset.MinValue;
			this.EndDateTime = DateTimeOffset.MinValue;
			this.Entries = [];
		}


		public CpuCoreHistoryResponse(int cpuIndex, int coreIndex, DateTimeOffset startDateTime, DateTimeOffset endDateTime, List<CpuCoreHistoryEntry> entries)
		{
			this.CpuIndex = cpuIndex;
			this.CoreIndex = coreIndex;
			this.StartDateTime = startDateTime;
			this.EndDateTime = endDateTime;
			this.Entries = [.. entries.Select(v => new CpuCoreHistoryEntryResponse(v))];
		}
	}
}
