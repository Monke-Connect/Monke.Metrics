using Hardware.Info;

using Microsoft.EntityFrameworkCore;

using Monke.Metrics.Caches;
using Monke.Metrics.Database;
using Monke.Metrics.Dtos.Cpu;
using Monke.Metrics.Exceptions;
using Monke.Metrics.Extensions;
using Monke.Metrics.Models.Cpu;

namespace Monke.Metrics.Services
{
	[RegisterService(ServiceLifetime.Scoped, typeof(ICpuService))]
	public class CpuService(ICacheServiceCollection caches, MetricsDbContext dbContext) : ICpuService
	{
		private static readonly TimeSpan MaxHistoryRange = TimeSpan.FromDays(14);
		private readonly ICacheServiceCollection caches = caches ?? throw new ArgumentNullException(nameof(caches));
		private readonly MetricsDbContext dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));


		public List<CpuInfoResponse> GetAllCpuInfos()
		{
			List<CpuInfoResponse> out_list = [];
			(IReadOnlyList<CPU> cpus, DateTime lastUpdated) = this.caches.CpusCache.Get();
			for (int i = 0; i < cpus.Count; i++)
			{
				out_list.Add(new CpuInfoResponse(i, cpus[i], lastUpdated));
			}
			return out_list;
		}


		public CpuInfoResponse GetSingleCpuInfo(int index)
		{
			if (index < 0)
				throw new BadRequestException($"Requested CPU index {index} is below zero.");

			(IReadOnlyList<CPU> cpus, DateTime lastUpdated) = this.caches.CpusCache.Get();
			try
			{
				return new CpuInfoResponse(index, cpus[index], lastUpdated);
			}
			catch (ArgumentOutOfRangeException)
			{
				throw new NotFoundException($"Requested CPU index {index} does not exist.");
			}
		}


		public CpuHistoryResponse GetSingleCpuHistory(int index, DateTimeOffset startDatetime, DateTimeOffset endDatetime)
		{
			// Validate parameters
			if (index < 0)
				throw new BadRequestException($"Requested CPU index {index} is below zero..");

			if (endDatetime < startDatetime)
				throw new BadRequestException($"End time can't be before start time.");

			if (endDatetime - startDatetime > MaxHistoryRange)
				throw new BadRequestException($"Requested time span can't be bigger than 14 days.");

			// Read data from database and reformat 
			IQueryable<CpuHistoryEntry> data = this.dbContext.CpuHistory
				.AsNoTracking()
				.Where(entry => entry.CpuIndex == index)
				.Where(entry => entry.Timestamp >= startDatetime)
				.Where(entry => entry.Timestamp <= endDatetime)
				.OrderBy(entry => entry.Timestamp);
			List<CpuHistoryEntry> out_data = [.. data];

			// Create response object and return it
			return new CpuHistoryResponse(index, startDatetime, endDatetime, out_data);
		}

		public List<CpuCoreInfoResponse> GetAllCpuCores(int cpuIndex)
		{
			if (cpuIndex < 0)
				throw new BadRequestException($"Requested CPU index {cpuIndex} is below zero..");

			List<CpuCoreInfoResponse> out_list = [];
			(IReadOnlyList<CPU> cpus, DateTime lastUpdated) = this.caches.CpusCache.Get();
			try
			{
				CPU cpu = cpus[cpuIndex];
				for (int i = 0; i < cpu.CpuCoreList.Count; i++)
				{
					out_list.Add(new CpuCoreInfoResponse(cpuIndex, i, cpu.CpuCoreList[i], lastUpdated));
				}
				return out_list;
			}
			catch (ArgumentOutOfRangeException)
			{
				throw new NotFoundException($"Requested CPU index {cpuIndex} does not exist.");
			}
		}

		public CpuCoreInfoResponse GetSingleCpuCore(int cpuIndex, int coreIndex)
		{
			if (cpuIndex < 0)
				throw new BadRequestException($"Requested CPU index {cpuIndex} is below zero..");

			if (coreIndex < 0)
				throw new BadRequestException($"Requested CPU-Core index {coreIndex} is below zero..");

			(IReadOnlyList<CPU> cpus, DateTime lastUpdated) = this.caches.CpusCache.Get();
			try
			{
				CPU cpu = cpus[cpuIndex];
				return new CpuCoreInfoResponse(cpuIndex, coreIndex, cpu.CpuCoreList[coreIndex], lastUpdated);
			}
			catch (ArgumentOutOfRangeException)
			{
				throw new NotFoundException($"Requested CPU index {cpuIndex} or CPU-Core index {coreIndex} does not exist.");
			}
		}

		public CpuCoreHistoryResponse GetSingleCpuCoreHistory(int cpuIndex, int coreIndex, DateTimeOffset startDatetime, DateTimeOffset endDatetime)
		{
			// Validate parameters
			if (cpuIndex < 0)
				throw new BadRequestException($"Requested CPU index {cpuIndex} is below zero..");

			if (coreIndex < 0)
				throw new BadRequestException($"Requested CPU-Core index {coreIndex} is below zero..");

			if (endDatetime < startDatetime)
				throw new BadRequestException($"End time can't be before start time.");

			if (endDatetime - startDatetime > MaxHistoryRange)
				throw new BadRequestException($"Requested time span can't be bigger than 14 days.");

			// Query database for CPU-Core history data and reformat it
			IQueryable<CpuCoreHistoryEntry> data = this.dbContext.CpuCoreHistory
				.AsNoTracking()
				.Where(entry => entry.CpuIndex == cpuIndex)
				.Where(entry => entry.CoreIndex == coreIndex)
				.Where(entry => entry.Timestamp >= startDatetime)
				.Where(entry => entry.Timestamp <= endDatetime)
				.OrderBy(entry => entry.Timestamp);
			List<CpuCoreHistoryEntry> out_data = [.. data];

			if (out_data.Count == 0)
				throw new NotFoundException($"No CPU-Core history found for CPU {cpuIndex} and Core {coreIndex}.");

			// Return result
			return new CpuCoreHistoryResponse(cpuIndex, coreIndex, startDatetime, endDatetime, out_data);
		}
	}
}
