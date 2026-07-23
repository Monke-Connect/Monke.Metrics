using Hardware.Info;

using Microsoft.EntityFrameworkCore;

using Monke.Metrics.Caches;
using Monke.Metrics.Database;
using Monke.Metrics.Dtos.Memory;
using Monke.Metrics.Exceptions;
using Monke.Metrics.Extensions;
using Monke.Metrics.Models.Memory;

namespace Monke.Metrics.Services
{
	[RegisterService(ServiceLifetime.Scoped, typeof(IMemoryService))]
	public class MemoryService(ICacheServiceCollection caches, MetricsDbContext dbContext) : IMemoryService
	{
		private static readonly TimeSpan MaxHistoryRange = TimeSpan.FromDays(14);
		private readonly ICacheServiceCollection caches = caches ?? throw new ArgumentNullException(nameof(caches));
		private readonly MetricsDbContext dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

		public List<MemoryInfoResponse> GetAllMemoryInfos()
		{
			List<MemoryInfoResponse> memoryInfos = [];
			(IReadOnlyList<Hardware.Info.Memory> value, DateTime lastUpdated) = this.caches.MemoryCache.Get();
			for (int i = 0; i < value.Count; i++)
			{
				memoryInfos.Add(new MemoryInfoResponse(i, value[i], lastUpdated));
			}
			return memoryInfos;
		}

		public MemoryInfoResponse GetSingleMemoryInfo(int index)
		{
			if (index < 0)
				throw new BadRequestException($"Requested memory index {index} is below zero.");

			(IReadOnlyList<Hardware.Info.Memory> value, DateTime lastUpdated) = this.caches.MemoryCache.Get();
			try
			{
				return new MemoryInfoResponse(index, value[index], lastUpdated);
			}
			catch (ArgumentOutOfRangeException)
			{
				throw new NotFoundException($"Requested CPU index {index} does not exist.");
			}
		}

		public MemoryHistoryResponse GetMemoryHistory(DateTimeOffset startDatetime, DateTimeOffset endDatetime)
		{
			// Validate parameters
			if (endDatetime < startDatetime)
				throw new BadRequestException($"End time can't be before start time.");

			if (endDatetime - startDatetime > MaxHistoryRange)
				throw new BadRequestException($"Requested time span can't be bigger than 14 days.");

			// Read data from database and reformat 
			IQueryable<MemoryHistoryEntry> data = this.dbContext.MemoryHistory
				.AsNoTracking()
				.Where(entry => entry.Timestamp >= startDatetime)
				.Where(entry => entry.Timestamp <= endDatetime)
				.OrderBy(entry => entry.Timestamp);
			List<MemoryHistoryEntry> out_data = [.. data];

			// Create response object and return it
			(MemoryStatus status, DateTime LastUpdated) = this.caches.MemoryStatusCache.Get();
			return new MemoryHistoryResponse(startDatetime, endDatetime, status, out_data);
		}
	}
}
