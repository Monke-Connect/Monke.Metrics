using Microsoft.AspNetCore.Mvc;

using Monke.Metrics.Dtos.Cpu;
using Monke.Metrics.Services;

namespace Monke.Metrics.Controllers
{
	/// <summary>
	/// Provides endpoints for reading CPU and CPU-core information and historical metrics.
	/// </summary>
	[Route("monke/metrics/cpus")]
	[ApiController]
	public class CpuController(ILogger<CpuController> logger, ICpuService cpuService) : ControllerBase
	{
		private readonly ILogger<CpuController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
		private readonly ICpuService _cpuService = cpuService ?? throw new ArgumentNullException(nameof(cpuService));


		/// <summary>
		/// Gets information for all detected CPUs on the host system.
		/// </summary>
		[HttpGet]
		[ProducesResponseType(typeof(List<CpuInfoResponse>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		public IActionResult GetAllCpus()
		{
			List<CpuInfoResponse> cpuInfoResponses = this._cpuService.GetAllCpuInfos();
			if (cpuInfoResponses.Count == 0)
			{
				this._logger.LogWarning("No CPU information available.");
				return this.NoContent();
			}
			return this.Ok(cpuInfoResponses);
		}


		/// <summary>
		/// Gets information for a single CPU by its index.
		/// </summary>
		[HttpGet("{cpuIndex:int}")]
		[ProducesResponseType(typeof(CpuInfoResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public IActionResult GetSingleCpu(int cpuIndex)
		{
			CpuInfoResponse response = this._cpuService.GetSingleCpuInfo(cpuIndex);
			return this.Ok(response);
		}


		/// <summary>
		/// Gets the historical metrics for a single CPU within a given time range.
		/// </summary>
		[HttpGet("{cpuIndex:int}/history")]
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

			CpuHistoryResponse cpuHistoryResponse = await this._cpuService.GetSingleCpuHistory(cpuIndex, start.Value, end.Value);
			return this.Ok(cpuHistoryResponse);
		}


		/// <summary>
		/// Gets information for all cores of a single CPU.
		/// </summary>
		[HttpGet("{cpuIndex:int}/cores")]
		[ProducesResponseType(typeof(List<CpuCoreInfoResponse>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		public IActionResult GetAllCpuCores(int cpuIndex)
		{
			List<CpuCoreInfoResponse> coreResponses = this._cpuService.GetAllCpuCores(cpuIndex);
			if (coreResponses.Count == 0)
			{
				this._logger.LogWarning("No CPU-Core information available.");
				return this.NoContent();
			}
			return this.Ok(coreResponses);
		}


		/// <summary>
		/// Gets information for a single core of a single CPU.
		/// </summary>
		[HttpGet("{cpuIndex:int}/cores/{coreIndex}")]
		[ProducesResponseType(typeof(CpuCoreInfoResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public IActionResult GetSingleCpuCore(int cpuIndex, int coreIndex)
		{
			CpuCoreInfoResponse response = this._cpuService.GetSingleCpuCore(cpuIndex, coreIndex);
			return this.Ok(response);
		}


		/// <summary>
		/// Gets the historical metrics for a single CPU core within a given time range.
		/// </summary>
		[HttpGet("{cpuIndex:int}/cores/{coreIndex}/history")]
		public IActionResult GetSingleCpuCoreHistory(int cpuIndex, int coreIndex,
			[FromQuery(Name = "start")] DateTimeOffset? start,
			[FromQuery(Name = "end")] DateTimeOffset? end)
		{
			if (!start.HasValue)
				start = DateTimeOffset.UtcNow.AddHours(-1);

			if (!end.HasValue)
				end = DateTimeOffset.UtcNow;

			CpuCoreHistoryResponse cpuCoreHistoryResponse = this._cpuService.GetSingleCpuCoreHistory(cpuIndex, coreIndex, start.Value, end.Value);
			return this.Ok(cpuCoreHistoryResponse);
		}
	}
}