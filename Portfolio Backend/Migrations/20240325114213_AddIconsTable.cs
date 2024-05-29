using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Portfolio_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddIconsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string[]>(
                name: "Icons",
                table: "Projects",
                type: "text[]",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Icons",
                table: "Projects");
        }
    }
}
