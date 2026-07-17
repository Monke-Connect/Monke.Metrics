using Monke.Metrics.Dtos.Cpu;

namespace Monke.Metrics.Services
{
	public interface ICpuService
	{
		List<CpuInfoResponse> GetAllCpuInfos();
		CpuInfoResponse GetSingleCpuInfo(int index);
		Task<CpuHistoryResponse> GetSingleCpuHistory(int index, DateTimeOffset startDatetime, DateTimeOffset endDatetime);
	}
}
