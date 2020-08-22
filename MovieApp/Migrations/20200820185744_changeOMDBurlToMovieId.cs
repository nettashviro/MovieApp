using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieApp.Migrations
{
    public partial class changeOMDBurlToMovieId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OMDBUrl",
                table: "Movie");

            migrationBuilder.AddColumn<int>(
                name: "MovieIdInOmdb",
                table: "Movie",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MovieIdInOmdb",
                table: "Movie");

            migrationBuilder.AddColumn<string>(
                name: "OMDBUrl",
                table: "Movie",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
