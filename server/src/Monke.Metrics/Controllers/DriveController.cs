using Microsoft.AspNetCore.Mvc;

using Monke.Metrics.Dtos.Drives;
using Monke.Metrics.Services;

namespace Monke.Metrics.Controllers
{
	[Route("monke/metrics/drives")]
	[ApiController]
	public class DriveController(ILogger<CpuController> logger, IDriveService driveService) : ControllerBase
	{
		private readonly ILogger<CpuController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

		private readonly IDriveService _driveService = driveService ?? throw new ArgumentNullException(nameof(driveService));


		/// <summary>
		/// Gets information for all detected drives on the host system.
		/// </summary>
		[HttpGet]
		[ProducesResponseType(typeof(List<DriveInfoResponse>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		public IActionResult GetAllDrives()
		{
			List<DriveInfoResponse> responses = this._driveService.GetAllDriveInfos();
			if (responses.Count == 0)
			{
				this._logger.LogWarning("No Drive information available.");
				return this.NoContent();
			}
			return this.Ok(responses);
		}


		/// <summary>
		/// Gets information for a single Drive by its index.
		/// </summary>
		[HttpGet("{driveIndex:int}")]
		[ProducesResponseType(typeof(DriveInfoResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public IActionResult GetSingleDrive(int driveIndex)
		{
			DriveInfoResponse response = this._driveService.GetSingleDriveInfo(driveIndex);
			return this.Ok(response);
		}


		/// <summary>
		/// Gets information for all partitions of a single drive.
		/// </summary>
		[HttpGet("{driveIndex:int}/partitions")]
		[ProducesResponseType(typeof(List<PartitionInfoResponse>), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		public IActionResult GetAllPartitions(int driveIndex)
		{
			List<PartitionInfoResponse> responses = this._driveService.GetAllPartitionInfos(driveIndex);
			if (responses.Count == 0)
			{
				this._logger.LogWarning("No CPU-Core information available.");
				return this.NoContent();
			}
			return this.Ok(responses);
		}

		/// <summary>
		/// Gets information for a single core of a single CPU.
		/// </summary>
		[HttpGet("{driveIndex:int}/partitions/{partitionIndex:int}")]
		[ProducesResponseType(typeof(PartitionInfoResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public IActionResult GetSinglePartitions(int driveIndex, int partitionIndex)
		{
			PartitionInfoResponse response = this._driveService.GetSinglePartitionInfo(driveIndex, partitionIndex);
			return this.Ok(response);
		}
	}
}
