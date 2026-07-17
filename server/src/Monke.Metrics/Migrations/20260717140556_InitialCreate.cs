using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Monke.Metrics.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CpuCoreHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CpuIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    CoreIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    PercentProcessorTime = table.Column<ulong>(type: "INTEGER", nullable: false),
                    Timestamp = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CpuCoreHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CpuHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CpuIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    CurrentClockSpeed = table.Column<uint>(type: "INTEGER", nullable: false),
                    PercentProcessorTime = table.Column<ulong>(type: "INTEGER", nullable: false),
                    Timestamp = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CpuHistory", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CpuCoreHistory");

            migrationBuilder.DropTable(
                name: "CpuHistory");
        }
    }
}
