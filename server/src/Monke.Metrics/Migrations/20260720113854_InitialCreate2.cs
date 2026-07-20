using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Monke.Metrics.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemoryHistory");
        }
    }
}
