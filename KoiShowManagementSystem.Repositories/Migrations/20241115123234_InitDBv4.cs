using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KoiShowManagementSystem.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class InitDBv4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnventsId",
                table: "JudgeAssignments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EnventsId",
                table: "JudgeAssignments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
