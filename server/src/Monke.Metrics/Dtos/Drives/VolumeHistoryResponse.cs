using Monke.Metrics.Models.Drives;

namespace Monke.Metrics.Dtos.Drives
{
	public sealed record VolumeHistoryResponse
	{

		public string Name { get; init; }

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
		public List<VolumeHistoryEntryResponse> Entries { get; init; }

		public VolumeHistoryResponse()
		{
			this.Name = string.Empty;
			this.StartDateTime = DateTimeOffset.MinValue;
			this.EndDateTime = DateTimeOffset.MinValue;
			this.Entries = [];
		}

		public VolumeHistoryResponse(string volumename, DateTimeOffset start, DateTimeOffset end, List<VolumeHistoryEntry> entries)
		{
			this.Name = volumename;
			this.StartDateTime = start;
			this.EndDateTime = end;
			this.Entries = [.. entries.Select(v => new VolumeHistoryEntryResponse(v))];
		}
	}
}
