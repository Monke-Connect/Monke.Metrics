using Monke.Metrics.Dtos.Memory;

namespace Monke.Metrics.Services
{
	public interface IMemoryService
	{
		List<MemoryInfoResponse> GetAllMemoryInfos();
		MemoryHistoryResponse GetMemoryHistory(DateTimeOffset startDatetime, DateTimeOffset endDatetime);
		MemoryInfoResponse GetSingleMemoryInfo(int index);
	}
}
