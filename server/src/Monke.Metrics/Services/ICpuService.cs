using Monke.Metrics.Dtos.Cpus;

namespace Monke.Metrics.Services
{
	public interface ICpuService
	{
		List<CpuInfoResponse> GetAllCpuInfos();
		CpuInfoResponse GetSingleCpuInfo(int index);
		CpuHistoryResponse GetSingleCpuHistory(int index, DateTimeOffset startDatetime, DateTimeOffset endDatetime);
		List<CpuCoreInfoResponse> GetAllCpuCores(int cpuIndex);
		CpuCoreInfoResponse GetSingleCpuCore(int cpuIndex, int coreIndex);
		CpuCoreHistoryResponse GetSingleCpuCoreHistory(int cpuIndex, int coreIndex, DateTimeOffset startDatetime, DateTimeOffset endDatetime);
	}
}
