using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Monke.Metrics.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
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

            migrationBuilder.CreateTable(
                name: "MemoryHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AvailablePhysical = table.Column<ulong>(type: "INTEGER", nullable: false),
                    AvailablePageFile = table.Column<ulong>(type: "INTEGER", nullable: false),
                    AvailableVirtual = table.Column<ulong>(type: "INTEGER", nullable: false),
                    Timestamp = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemoryHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VolumeHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    FreeSpace = table.Column<ulong>(type: "INTEGER", nullable: false),
                    Timestamp = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VolumeHistory", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CpuCoreHistory_CpuIndex_CoreIndex_Timestamp",
                table: "CpuCoreHistory",
                columns: new[] { "CpuIndex", "CoreIndex", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_CpuHistory_CpuIndex_Timestamp",
                table: "CpuHistory",
                columns: new[] { "CpuIndex", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_VolumeHistory_Name_Timestamp",
                table: "VolumeHistory",
                columns: new[] { "Name", "Timestamp" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CpuCoreHistory");

            migrationBuilder.DropTable(
                name: "CpuHistory");

            migrationBuilder.DropTable(
                name: "MemoryHistory");

            migrationBuilder.DropTable(
                name: "VolumeHistory");
        }
    }
}
