using Microsoft.EntityFrameworkCore;

using Monke.Metrics.Models.Cpu;

namespace Monke.Metrics.Data
{
	public class MetricsDbContext(DbContextOptions<MetricsDbContext> options) : DbContext(options)
	{
		public DbSet<CpuHistoryEntry> CpuHistory => this.Set<CpuHistoryEntry>();
		public DbSet<CpuCoreHistoryEntry> CpuCoreHistory => this.Set<CpuCoreHistoryEntry>();

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			_ = modelBuilder.Entity<CpuHistoryEntry>()
				.Property(e => e.Timestamp)
				.HasConversion(
					v => v.UtcTicks,
					v => new DateTimeOffset(v, TimeSpan.Zero));

			_ = modelBuilder.Entity<CpuCoreHistoryEntry>()
				.Property(e => e.Timestamp)
				.HasConversion(
					v => v.UtcTicks,
					v => new DateTimeOffset(v, TimeSpan.Zero));

			base.OnModelCreating(modelBuilder);
		}
	}
}