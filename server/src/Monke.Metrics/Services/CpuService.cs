using Hardware.Info;

using Microsoft.EntityFrameworkCore;

using Monke.Metrics.Caches;
using Monke.Metrics.Data;
using Monke.Metrics.Dtos.Cpu;
using Monke.Metrics.Exceptions;
using Monke.Metrics.Extensions;
using Monke.Metrics.Models.Cpu;

namespace Monke.Metrics.Services
{
	[RegisterService(ServiceLifetime.Scoped, typeof(ICpuService))]
	public class CpuService(ILogger<CpuService> logger, ICacheServiceCollection caches, MetricsDbContext dbContext) : ICpuService
	{
		private static readonly TimeSpan MaxHistoryRange = TimeSpan.FromDays(14);
		private readonly ILogger<CpuService> logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
				throw new BadRequestException($"Requested CPU index {index} is below zero..");

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


		public async Task<CpuHistoryResponse> GetSingleCpuHistory(int index, DateTimeOffset startDatetime, DateTimeOffset endDatetime)
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

			if (out_data.Count == 0)
				throw new NotFoundException($"No CPU history found for CPU {index}.");

			// Create response object and return it
			return new CpuHistoryResponse(index, startDatetime, endDatetime, out_data);
		}
	}
}
