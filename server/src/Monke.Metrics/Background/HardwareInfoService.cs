using System.Collections.ObjectModel;
using System.Management;

using Hardware.Info;

using Microsoft.EntityFrameworkCore;

using Monke.Metrics.Caches;
using Monke.Metrics.Data;
using Monke.Metrics.Models.Cpu;

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
			}
		}


		private void RefreshHardwareInfo()
		{
			if (this.logger.IsEnabled(LogLevel.Information))
				this.logger.LogInformation("Refreshing hardware info.");

			this.hardwareInfo.RefreshCPUList();
			return;
		}


		private async Task UpdateCpuValuesAsync(MetricsDbContext context, CancellationToken stoppingToken)
		{
			// Update the cpu info that is saved in the cache
			if (this.logger.IsEnabled(LogLevel.Debug))
				this.logger.LogInformation("Updating cached cpu values.");

			ReadOnlyCollection<CPU> cpus = this.hardwareInfo.CpuList.AsReadOnly();
			this.caches.CpusCache.Set(cpus); // lock in there

			// Update the cpu history that is saved in the database
			if (this.logger.IsEnabled(LogLevel.Debug))
				this.logger.LogInformation("Updating persisted cpu history.");
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
	}
}
