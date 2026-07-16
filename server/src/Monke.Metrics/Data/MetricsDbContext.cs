using Microsoft.EntityFrameworkCore;

namespace Monke.Metrics.Data
{
	public class MetricsDbContext(DbContextOptions<MetricsDbContext> options) : DbContext(options)
	{
	}
}