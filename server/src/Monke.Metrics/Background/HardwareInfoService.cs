using System.Collections.ObjectModel;
using System.Management;

using Hardware.Info;

using Microsoft.EntityFrameworkCore;

using Monke.Metrics.Caches;
using Monke.Metrics.Database;
using Monke.Metrics.Models.Cpu;
using Monke.Metrics.Models.Drives;
using Monke.Metrics.Models.Memory;

namespace Monke.Metrics.Background
{
	public class HardwareInfoService(
		ICacheServiceCollection caches,
		ILogger<HardwareInfoService> logger,
		IServiceScopeFactory scopeFactory) : BackgroundService
	{
		private static readonly TimeSpan WmiDelay = TimeSpan.FromSeconds(3);
		private static readonly TimeSpan RefreshInterval = TimeSpan.FromSeconds(5);
		private readonly ICacheServiceCollection caches = caches;
		private readonly ILogger<HardwareInfoService> logger = logger;
		private readonly IServiceScopeFactory scopeFactory = scopeFactory;
		private readonly HardwareInfo hardwareInfo = new HardwareInfo(timeoutInWMI: WmiDelay);

		/// <inheritdoc/>
		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			try
			{
				// Initial refresh -> maybe 21 seconds on initial call on some systems (WMI limitation)
				this.hardwareInfo.RefreshAll();
			}
			catch (ManagementException)
			{
				this.logger.LogWarning("WMI Timeout while collecting hardware info");
			}


			using PeriodicTimer timer = new PeriodicTimer(RefreshInterval);
			while (await timer.WaitForNextTickAsync(stoppingToken))
			{
				this.RefreshHardwareInfo();

				// Create a new scope for the database context
				using IServiceScope scope = this.scopeFactory.CreateScope();
				MetricsDbContext context = scope.ServiceProvider.GetRequiredService<MetricsDbContext>();

				// Update the values in the cache and database
				await this.UpdateCpuValuesAsync(context, stoppingToken);
				await this.UpdateMemoryValuesAsync(context, stoppingToken);
				await this.UpdateDriveValuesAsync(context, stoppingToken);
			}
		}


		private void RefreshHardwareInfo()
		{
			if (this.logger.IsEnabled(LogLevel.Information))
				this.logger.LogInformation("Refreshing hardware info.");

			this.hardwareInfo.RefreshCPUList();
			this.hardwareInfo.RefreshMemoryStatus();
			this.hardwareInfo.RefreshMemoryList();
			this.hardwareInfo.RefreshDriveList();
			return;
		}


		private async Task UpdateCpuValuesAsync(MetricsDbContext context, CancellationToken stoppingToken)
		{
			// Update the cpu info that is saved in the cache
			if (this.logger.IsEnabled(LogLevel.Debug))
				this.logger.LogDebug("Updating cached cpu values.");

			ReadOnlyCollection<CPU> cpus = this.hardwareInfo.CpuList.AsReadOnly();
			this.caches.CpusCache.Set(cpus); // lock in there

			// Update the cpu history that is saved in the database
			if (this.logger.IsEnabled(LogLevel.Debug))
				this.logger.LogDebug("Updating persisted cpu history.");
			try
			{
				for (int cpuIndex = 0; cpuIndex < cpus.Count; cpuIndex++)
				{
					// Table: CpuHistory
					CPU cpu = cpus[cpuIndex];
					_ = await context.CpuHistory.AddAsync(new CpuHistoryEntry(cpuIndex, cpu), stoppingToken);

					// Table: CpuCoreHistory
					for (int coreIndex = 0; coreIndex < cpus[cpuIndex].CpuCoreList.Count; coreIndex++)
					{
						_ = await context.CpuCoreHistory.AddAsync(new CpuCoreHistoryEntry(cpuIndex, coreIndex, cpu.CpuCoreList[coreIndex]), stoppingToken);
					}
				}
				_ = await context.SaveChangesAsync(stoppingToken);
			}
			catch (DbUpdateException ex)
			{
				this.logger.LogError(ex, "An error occurred while updating the database.");
			}
		}


		private async Task UpdateMemoryValuesAsync(MetricsDbContext context, CancellationToken stoppingToken)
		{
			// Update the memory info that is saved in the cache
			if (this.logger.IsEnabled(LogLevel.Debug))
				this.logger.LogDebug("Updating cached memory values.");

			// Update cached status instance
			MemoryStatus memoryStatus = this.hardwareInfo.MemoryStatus;
			this.caches.MemoryStatusCache.Set(memoryStatus); // lock in there

			// Update cached memory instances
			ReadOnlyCollection<Memory> memories = this.hardwareInfo.MemoryList.AsReadOnly();
			this.caches.MemoryCache.Set(memories); // lock in there

			// Update the memory history that is saved in the database
			if (this.logger.IsEnabled(LogLevel.Debug))
				this.logger.LogInformation("Updating persisted memory history.");
			try
			{
				// Table: MemoryHistory
				_ = await context.MemoryHistory.AddAsync(new MemoryHistoryEntry(memoryStatus), stoppingToken);
				_ = await context.SaveChangesAsync(stoppingToken);
			}
			catch (DbUpdateException ex)
			{
				this.logger.LogError(ex, "An error occurred while updating the database.");
			}
		}

		private List<Volume> GetUniqueVolumes(IReadOnlyList<Drive> drives)
		{
			Dictionary<string, Volume> uniqueVolumes = [];
			foreach (Drive drive in drives)
			{
				foreach (Partition partition in drive.PartitionList)
				{
					foreach (Volume volume in partition.VolumeList)
					{
						bool added = uniqueVolumes.TryAdd(volume.Name, volume);
						if (!added && this.logger.IsEnabled(LogLevel.Debug))
							this.logger.LogDebug("Duplicate volume {name} found.", volume.Name);
					}
				}
			}
			return [.. uniqueVolumes.Values];
		}

		private async Task UpdateDriveValuesAsync(MetricsDbContext context, CancellationToken stoppingToken)
		{
			// Update the memory info that is saved in the cache
			if (this.logger.IsEnabled(LogLevel.Debug))
				this.logger.LogDebug("Updating cached drive values.");

			// Update cached drive instances
			IReadOnlyList<Drive> drives = this.hardwareInfo.DriveList.AsReadOnly();
			this.caches.DrivesCache.Set(drives); // lock in there

			// Update the memory history that is saved in the database
			if (this.logger.IsEnabled(LogLevel.Debug))
				this.logger.LogInformation("Updating persisted volume history.");
			try
			{
				// Table: VolumeHistory
				List<Volume> uniqueVolumes = this.GetUniqueVolumes(drives);
				foreach (Volume volume in uniqueVolumes)
				{
					_ = await context.VolumeHistory.AddAsync(new VolumeHistoryEntry(volume), stoppingToken);
				}
				_ = await context.SaveChangesAsync(stoppingToken);
			}
			catch (DbUpdateException ex)
			{
				this.logger.LogError(ex, "An error occurred while updating the database.");
			}
		}
	}
}
