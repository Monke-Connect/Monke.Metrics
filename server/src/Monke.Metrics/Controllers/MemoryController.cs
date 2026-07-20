using Microsoft.AspNetCore.Mvc;

using Monke.Metrics.Dtos.Cpu;
using Monke.Metrics.Dtos.Memory;
using Monke.Metrics.Services;

namespace Monke.Metrics.Controllers
{
	/// <summary>
	/// Provides endpoints for reading memory information and historical metrics.
	/// </summary>
	[Route("monke/metrics/memories")]
	[ApiController]
	public class MemoryController(ILogger<MemoryController> logger, IMemoryService memoryService) : ControllerBase
	{
		private readonly ILogger<MemoryController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
		private readonly IMemoryService _memoryService = memoryService ?? throw new ArgumentNullException(nameof(memoryService));

		/// <summary>
		/// Gets information for all detected CPUs on the host system.
		/// </summary>
		[HttpGet]
		[ProducesResponseType(typeof(List<MemoryInfoResponse>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		public IActionResult GetAllMemories()
		{
			List<MemoryInfoResponse> cpuInfoResponses = this._memoryService.GetAllMemoryInfos();
			if (cpuInfoResponses.Count == 0)
			{
				this._logger.LogWarning("No memory information available.");
				return this.NoContent();
			}
			return this.Ok(cpuInfoResponses);
		}


		/// <summary>
		/// Gets information for a single memory by its index.
		/// </summary>
		[HttpGet("{memoryIndex:int}")]
		[ProducesResponseType(typeof(MemoryInfoResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public IActionResult GetSingleMemory(int memoryIndex)
		{
			MemoryInfoResponse response = this._memoryService.GetSingleMemoryInfo(memoryIndex);
			return this.Ok(response);
		}


		/// <summary>
		/// Gets the historical metrics for the memory status within a given time range.
		/// </summary>
		[HttpGet("history")]
		[ProducesResponseType(typeof(CpuHistoryResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetSingleCpuHistoryAsync(int cpuIndex,
			[FromQuery(Name = "start")] DateTimeOffset? start,
			[FromQuery(Name = "end")] DateTimeOffset? end)
		{
			if (!start.HasValue)
				start = DateTimeOffset.UtcNow.AddHours(-1);

			if (!end.HasValue)
				end = DateTimeOffset.UtcNow;

			MemoryHistoryResponse response = this._memoryService.GetMemoryHistory(start.Value, end.Value);
			return this.Ok(response);
		}
	}
}
