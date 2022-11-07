using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyFavoriteMovie.Core.Migrations
{
    public partial class add_auth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFavoriteActor_Users_AccountsFavoriteId",
                table: "UserFavoriteActor");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFavoriteMovie_Users_AccountsFavoriteId",
                table: "UserFavoriteMovie");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserFavoriteMovie",
                table: "UserFavoriteMovie");

            migrationBuilder.DropIndex(
                name: "IX_UserFavoriteMovie_FavoriteMoviesId",
                table: "UserFavoriteMovie");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserFavoriteActor",
                table: "UserFavoriteActor");

            migrationBuilder.DropIndex(
                name: "IX_UserFavoriteActor_FavoriteActorsId",
                table: "UserFavoriteActor");

            migrationBuilder.RenameColumn(
                name: "AccountsFavoriteId",
                table: "UserFavoriteMovie",
                newName: "UsersFavoriteId");

            migrationBuilder.RenameColumn(
                name: "AccountsFavoriteId",
                table: "UserFavoriteActor",
                newName: "UsersFavoriteId");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nickname",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RegistrationDate",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Surname",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserFavoriteMovie",
                table: "UserFavoriteMovie",
                columns: new[] { "FavoriteMoviesId", "UsersFavoriteId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserFavoriteActor",
                table: "UserFavoriteActor",
                columns: new[] { "FavoriteActorsId", "UsersFavoriteId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserFavoriteMovie_UsersFavoriteId",
                table: "UserFavoriteMovie",
                column: "UsersFavoriteId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFavoriteActor_UsersFavoriteId",
                table: "UserFavoriteActor",
                column: "UsersFavoriteId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFavoriteActor_Users_UsersFavoriteId",
                table: "UserFavoriteActor",
                column: "UsersFavoriteId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFavoriteMovie_Users_UsersFavoriteId",
                table: "UserFavoriteMovie",
                column: "UsersFavoriteId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFavoriteActor_Users_UsersFavoriteId",
                table: "UserFavoriteActor");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFavoriteMovie_Users_UsersFavoriteId",
                table: "UserFavoriteMovie");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserFavoriteMovie",
                table: "UserFavoriteMovie");

            migrationBuilder.DropIndex(
                name: "IX_UserFavoriteMovie_UsersFavoriteId",
                table: "UserFavoriteMovie");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserFavoriteActor",
                table: "UserFavoriteActor");

            migrationBuilder.DropIndex(
                name: "IX_UserFavoriteActor_UsersFavoriteId",
                table: "UserFavoriteActor");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Nickname",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RegistrationDate",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Surname",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "UsersFavoriteId",
                table: "UserFavoriteMovie",
                newName: "AccountsFavoriteId");

            migrationBuilder.RenameColumn(
                name: "UsersFavoriteId",
                table: "UserFavoriteActor",
                newName: "AccountsFavoriteId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserFavoriteMovie",
                table: "UserFavoriteMovie",
                columns: new[] { "AccountsFavoriteId", "FavoriteMoviesId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserFavoriteActor",
                table: "UserFavoriteActor",
                columns: new[] { "AccountsFavoriteId", "FavoriteActorsId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserFavoriteMovie_FavoriteMoviesId",
                table: "UserFavoriteMovie",
                column: "FavoriteMoviesId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFavoriteActor_FavoriteActorsId",
                table: "UserFavoriteActor",
                column: "FavoriteActorsId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFavoriteActor_Users_AccountsFavoriteId",
                table: "UserFavoriteActor",
                column: "AccountsFavoriteId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFavoriteMovie_Users_AccountsFavoriteId",
                table: "UserFavoriteMovie",
                column: "AccountsFavoriteId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
