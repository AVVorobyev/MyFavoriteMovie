using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyFavoriteMovie.Core.Migrations
{
    public partial class rename_Title_in_Description : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Movies",
                newName: "Description");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Movies",
                newName: "Title");
        }
    }
}
