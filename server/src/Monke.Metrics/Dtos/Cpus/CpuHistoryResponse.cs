using Monke.Metrics.Models.Cpu;

namespace Monke.Metrics.Dtos.Cpus
{
	public sealed record CpuHistoryResponse
	{
		/// <summary>
		/// Index of the CPU within the systems CPUs.
		/// </summary>
		public int CpuIndex { get; init; }

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
		public List<CpuHistoryEntryResponse> Entries { get; init; }


		public CpuHistoryResponse()
		{
			this.CpuIndex = 0;
			this.StartDateTime = DateTimeOffset.MinValue;
			this.EndDateTime = DateTimeOffset.MinValue;
			this.Entries = [];
		}


		public CpuHistoryResponse(int cpuIndex, DateTimeOffset startDateTime, DateTimeOffset endDateTime, List<CpuHistoryEntry> entries)
		{
			this.CpuIndex = cpuIndex;
			this.StartDateTime = startDateTime;
			this.EndDateTime = endDateTime;
			this.Entries = [.. entries.Select(v => new CpuHistoryEntryResponse(v))];
		}
	}
}
