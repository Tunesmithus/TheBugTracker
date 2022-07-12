using Microsoft.EntityFrameworkCore.Migrations;

namespace TheBugTracker.Data.Migrations
{
    public partial class ProjectPropertyNameChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileData",
                table: "Projects",
                newName: "ImageFileData");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageFileData",
                table: "Projects",
                newName: "FileData");
        }
    }
}
