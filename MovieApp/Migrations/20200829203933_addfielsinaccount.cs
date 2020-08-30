using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieApp.Migrations
{
    public partial class addfielsinaccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountId1",
                table: "Movie",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AccountId2",
                table: "Movie",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Movie_AccountId1",
                table: "Movie",
                column: "AccountId1");

            migrationBuilder.CreateIndex(
                name: "IX_Movie_AccountId2",
                table: "Movie",
                column: "AccountId2");

            migrationBuilder.AddForeignKey(
                name: "FK_Movie_Account_AccountId1",
                table: "Movie",
                column: "AccountId1",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Movie_Account_AccountId2",
                table: "Movie",
                column: "AccountId2",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movie_Account_AccountId1",
                table: "Movie");

            migrationBuilder.DropForeignKey(
                name: "FK_Movie_Account_AccountId2",
                table: "Movie");

            migrationBuilder.DropIndex(
                name: "IX_Movie_AccountId1",
                table: "Movie");

            migrationBuilder.DropIndex(
                name: "IX_Movie_AccountId2",
                table: "Movie");

            migrationBuilder.DropColumn(
                name: "AccountId1",
                table: "Movie");

            migrationBuilder.DropColumn(
                name: "AccountId2",
                table: "Movie");
        }
    }
}
