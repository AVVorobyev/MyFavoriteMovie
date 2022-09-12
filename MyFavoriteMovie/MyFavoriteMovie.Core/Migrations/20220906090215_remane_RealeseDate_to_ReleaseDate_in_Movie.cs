using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyFavoriteMovie.Core.Migrations
{
    public partial class remane_RealeseDate_to_ReleaseDate_in_Movie : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RealeseDate",
                table: "Movies",
                newName: "ReleaseDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReleaseDate",
                table: "Movies",
                newName: "RealeseDate");
        }
    }
}
