using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieApp.Migrations
{
    public partial class foringkey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movie_Account_AccountId",
                table: "Movie");

            migrationBuilder.DropForeignKey(
                name: "FK_Movie_Account_AccountId1",
                table: "Movie");

            migrationBuilder.DropForeignKey(
                name: "FK_Movie_Account_AccountId2",
                table: "Movie");

            migrationBuilder.DropIndex(
                name: "IX_Movie_AccountId",
                table: "Movie");

            migrationBuilder.DropIndex(
                name: "IX_Movie_AccountId1",
                table: "Movie");

            migrationBuilder.DropIndex(
                name: "IX_Movie_AccountId2",
                table: "Movie");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Movie");

            migrationBuilder.DropColumn(
                name: "AccountId1",
                table: "Movie");

            migrationBuilder.DropColumn(
                name: "AccountId2",
                table: "Movie");

            migrationBuilder.AddColumn<int>(
                name: "MovieClickedId",
                table: "Movie",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MovieWatchedId",
                table: "Movie",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MovieWatchlistId",
                table: "Movie",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Movie_MovieClickedId",
                table: "Movie",
                column: "MovieClickedId");

            migrationBuilder.CreateIndex(
                name: "IX_Movie_MovieWatchedId",
                table: "Movie",
                column: "MovieWatchedId");

            migrationBuilder.CreateIndex(
                name: "IX_Movie_MovieWatchlistId",
                table: "Movie",
                column: "MovieWatchlistId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movie_Account_MovieClickedId",
                table: "Movie",
                column: "MovieClickedId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Movie_Account_MovieWatchedId",
                table: "Movie",
                column: "MovieWatchedId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Movie_Account_MovieWatchlistId",
                table: "Movie",
                column: "MovieWatchlistId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movie_Account_MovieClickedId",
                table: "Movie");

            migrationBuilder.DropForeignKey(
                name: "FK_Movie_Account_MovieWatchedId",
                table: "Movie");

            migrationBuilder.DropForeignKey(
                name: "FK_Movie_Account_MovieWatchlistId",
                table: "Movie");

            migrationBuilder.DropIndex(
                name: "IX_Movie_MovieClickedId",
                table: "Movie");

            migrationBuilder.DropIndex(
                name: "IX_Movie_MovieWatchedId",
                table: "Movie");

            migrationBuilder.DropIndex(
                name: "IX_Movie_MovieWatchlistId",
                table: "Movie");

            migrationBuilder.DropColumn(
                name: "MovieClickedId",
                table: "Movie");

            migrationBuilder.DropColumn(
                name: "MovieWatchedId",
                table: "Movie");

            migrationBuilder.DropColumn(
                name: "MovieWatchlistId",
                table: "Movie");

            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "Movie",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AccountId1",
                table: "Movie",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AccountId2",
                table: "Movie",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Movie_AccountId",
                table: "Movie",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Movie_AccountId1",
                table: "Movie",
                column: "AccountId1");

            migrationBuilder.CreateIndex(
                name: "IX_Movie_AccountId2",
                table: "Movie",
                column: "AccountId2");

            migrationBuilder.AddForeignKey(
                name: "FK_Movie_Account_AccountId",
                table: "Movie",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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
    }
}
