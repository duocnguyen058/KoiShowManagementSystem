using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KoiShowManagementSystem.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class InitDBv5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoPath",
                table: "Kois",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoPath",
                table: "Kois");
        }
    }
}
