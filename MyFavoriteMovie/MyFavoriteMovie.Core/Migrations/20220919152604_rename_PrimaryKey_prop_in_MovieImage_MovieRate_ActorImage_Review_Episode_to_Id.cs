using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyFavoriteMovie.Core.Migrations
{
    public partial class rename_PrimaryKey_prop_in_MovieImage_MovieRate_ActorImage_Review_Episode_to_Id : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReviewId",
                table: "Reviews",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "MovieRateId",
                table: "MovieRates",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "MovieImageId",
                table: "MovieImage",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "EpisodeId",
                table: "Episodes",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ActorImageId",
                table: "ActorImage",
                newName: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Reviews",
                newName: "ReviewId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "MovieRates",
                newName: "MovieRateId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "MovieImage",
                newName: "MovieImageId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Episodes",
                newName: "EpisodeId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ActorImage",
                newName: "ActorImageId");
        }
    }
}
