using Microsoft.AspNetCore.Mvc;

using Monke.Metrics.Dtos.Drives;
using Monke.Metrics.Services;

namespace Monke.Metrics.Controllers
{
	[Route("monke/metrics/volumes")]
	[ApiController]
	public class VolumeController(ILogger<CpuController> logger, IDriveService driveService) : ControllerBase
	{
		private readonly ILogger<CpuController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

		private readonly IDriveService _driveService = driveService ?? throw new ArgumentNullException(nameof(driveService));

		/// <summary>
		/// Gets information for all detected volumes on the host system.
		/// </summary>
		[HttpGet]
		[ProducesResponseType(typeof(List<VolumeInfoResponse>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		public IActionResult GetAllVolumes()
		{
			List<VolumeInfoResponse> responses = this._driveService.GetAllVolumeInfos();
			if (responses.Count == 0)
			{
				this._logger.LogWarning("No volume information available.");
				return this.NoContent();
			}
			return this.Ok(responses);
		}

		/// <summary>
		/// Gets information for a single volume by its name.
		/// </summary>
		[HttpGet("{name}")]
		[ProducesResponseType(typeof(VolumeInfoResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public IActionResult GetSingleDrive(string name)
		{
			VolumeInfoResponse response = this._driveService.GetSingleVolumeInfo(name);
			return this.Ok(response);
		}
	}
}
