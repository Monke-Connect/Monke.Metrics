using Microsoft.EntityFrameworkCore;

using Monke.Metrics.Models.Cpu;
using Monke.Metrics.Models.Drives;
using Monke.Metrics.Models.Memory;

namespace Monke.Metrics.Database
{
	public class MetricsDbContext(DbContextOptions<MetricsDbContext> options) : DbContext(options)
	{
		public DbSet<CpuHistoryEntry> CpuHistory => this.Set<CpuHistoryEntry>();

		public DbSet<CpuCoreHistoryEntry> CpuCoreHistory => this.Set<CpuCoreHistoryEntry>();

		public DbSet<MemoryHistoryEntry> MemoryHistory => this.Set<MemoryHistoryEntry>();

		public DbSet<VolumeHistoryEntry> VolumeHistory => this.Set<VolumeHistoryEntry>();


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

			_ = modelBuilder.Entity<MemoryHistoryEntry>()
				.Property(e => e.Timestamp)
				.HasConversion(
					v => v.UtcTicks,
					v => new DateTimeOffset(v, TimeSpan.Zero));

			_ = modelBuilder.Entity<VolumeHistoryEntry>()
				.Property(e => e.Timestamp)
				.HasConversion(
					v => v.UtcTicks,
					v => new DateTimeOffset(v, TimeSpan.Zero));

			base.OnModelCreating(modelBuilder);
		}
	}
}