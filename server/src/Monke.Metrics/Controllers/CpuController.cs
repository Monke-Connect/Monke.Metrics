using Microsoft.AspNetCore.Mvc;

using Monke.Metrics.Dtos.Cpu;
using Monke.Metrics.Services;

namespace Monke.Metrics.Controllers
{
	[Route("monke/metrics/cpus")]
	[ApiController]
	public class CpuController(ILogger<CpuController> logger, ICpuService cpuService) : ControllerBase
	{
		private readonly ILogger<CpuController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
		private readonly ICpuService _cpuService = cpuService ?? throw new ArgumentNullException(nameof(cpuService));

		[HttpGet]
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


		[HttpGet("{index:int}")]
		public IActionResult GetSingleCpu(int index)
		{
			CpuInfoResponse response = this._cpuService.GetSingleCpuInfo(index);
			return this.Ok(response);
		}


		[HttpGet("{index:int}/history")]
		public async Task<IActionResult> GetSingleCpuHistoryAsync(int index,
			[FromQuery(Name = "start")] DateTimeOffset? start,
			[FromQuery(Name = "end")] DateTimeOffset? end)
		{
			if (!start.HasValue)
				start = DateTimeOffset.UtcNow.AddHours(-1);

			if (!end.HasValue)
				end = DateTimeOffset.UtcNow;

			CpuHistoryResponse cpuHistoryResponse = await this._cpuService.GetSingleCpuHistory(index, start.Value, end.Value);
			return this.Ok(cpuHistoryResponse);
		}

		[HttpGet("{index:int}/cores")]
		public IActionResult GetAllCpuCores(int index) => this.Ok();


		[HttpGet("{cpuIndex:int}/cores/{coreIndex}")]
		public IActionResult GetSingleCpuCore(int cpuIndex, int coreIndex) => this.Ok();


		[HttpGet("{cpuIndex:int}/cores/{coreIndex}/history")]
		public IActionResult GetSingleCpuCoreHistory(int cpuIndex, int coreIndex) => this.Ok();
	}
}
