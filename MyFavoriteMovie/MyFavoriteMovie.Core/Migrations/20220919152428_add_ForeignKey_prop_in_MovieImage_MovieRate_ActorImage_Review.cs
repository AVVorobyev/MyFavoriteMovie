using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyFavoriteMovie.Core.Migrations
{
    public partial class add_ForeignKey_prop_in_MovieImage_MovieRate_ActorImage_Review : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActorImage_Actors_Id",
                table: "ActorImage");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieImage_Movies_Id",
                table: "MovieImage");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieRates_Movies_Id",
                table: "MovieRates");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieRates_Users_Id",
                table: "MovieRates");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Movies_Id",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Users_Id",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieRates",
                table: "MovieRates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieImage",
                table: "MovieImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ActorImage",
                table: "ActorImage");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Reviews",
                newName: "MovieId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "MovieRates",
                newName: "MovieId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "MovieImage",
                newName: "MovieId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ActorImage",
                newName: "ActorId");

            migrationBuilder.AddColumn<int>(
                name: "ReviewId",
                table: "Reviews",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "Reviews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MovieRateId",
                table: "MovieRates",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "MovieRates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MovieImageId",
                table: "MovieImage",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "ActorImageId",
                table: "ActorImage",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews",
                column: "ReviewId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieRates",
                table: "MovieRates",
                column: "MovieRateId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieImage",
                table: "MovieImage",
                column: "MovieImageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActorImage",
                table: "ActorImage",
                column: "ActorImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_AuthorId",
                table: "Reviews",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_MovieId",
                table: "Reviews",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieRates_AccountId",
                table: "MovieRates",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieRates_MovieId",
                table: "MovieRates",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieImage_MovieId",
                table: "MovieImage",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_ActorImage_ActorId",
                table: "ActorImage",
                column: "ActorId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActorImage_Actors_ActorId",
                table: "ActorImage",
                column: "ActorId",
                principalTable: "Actors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieImage_Movies_MovieId",
                table: "MovieImage",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieRates_Movies_MovieId",
                table: "MovieRates",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieRates_Users_AccountId",
                table: "MovieRates",
                column: "AccountId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Movies_MovieId",
                table: "Reviews",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Users_AuthorId",
                table: "Reviews",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActorImage_Actors_ActorId",
                table: "ActorImage");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieImage_Movies_MovieId",
                table: "MovieImage");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieRates_Movies_MovieId",
                table: "MovieRates");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieRates_Users_AccountId",
                table: "MovieRates");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Movies_MovieId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Users_AuthorId",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_AuthorId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_MovieId",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieRates",
                table: "MovieRates");

            migrationBuilder.DropIndex(
                name: "IX_MovieRates_AccountId",
                table: "MovieRates");

            migrationBuilder.DropIndex(
                name: "IX_MovieRates_MovieId",
                table: "MovieRates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MovieImage",
                table: "MovieImage");

            migrationBuilder.DropIndex(
                name: "IX_MovieImage_MovieId",
                table: "MovieImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ActorImage",
                table: "ActorImage");

            migrationBuilder.DropIndex(
                name: "IX_ActorImage_ActorId",
                table: "ActorImage");

            migrationBuilder.DropColumn(
                name: "ReviewId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "MovieRateId",
                table: "MovieRates");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "MovieRates");

            migrationBuilder.DropColumn(
                name: "MovieImageId",
                table: "MovieImage");

            migrationBuilder.DropColumn(
                name: "ActorImageId",
                table: "ActorImage");

            migrationBuilder.RenameColumn(
                name: "MovieId",
                table: "Reviews",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "MovieId",
                table: "MovieRates",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "MovieId",
                table: "MovieImage",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ActorId",
                table: "ActorImage",
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieRates",
                table: "MovieRates",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MovieImage",
                table: "MovieImage",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActorImage",
                table: "ActorImage",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ActorImage_Actors_Id",
                table: "ActorImage",
                column: "Id",
                principalTable: "Actors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieImage_Movies_Id",
                table: "MovieImage",
                column: "Id",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieRates_Movies_Id",
                table: "MovieRates",
                column: "Id",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MovieRates_Users_Id",
                table: "MovieRates",
                column: "Id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Movies_Id",
                table: "Reviews",
                column: "Id",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Users_Id",
                table: "Reviews",
                column: "Id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
