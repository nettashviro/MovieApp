using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieApp.Migrations
{
    public partial class changeFromOMDBToTMDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MovieIdInOmdb",
                table: "Movie");

            migrationBuilder.AddColumn<int>(
                name: "MovieIdInTMDB",
                table: "Movie",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MovieIdInTMDB",
                table: "Movie");

            migrationBuilder.AddColumn<int>(
                name: "MovieIdInOmdb",
                table: "Movie",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
