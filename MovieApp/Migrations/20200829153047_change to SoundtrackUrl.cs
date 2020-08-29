using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieApp.Migrations
{
    public partial class changetoSoundtrackUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrailerUrl",
                table: "Soundtrack");

            migrationBuilder.AddColumn<string>(
                name: "SoundtrackUrl",
                table: "Soundtrack",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SoundtrackUrl",
                table: "Soundtrack");

            migrationBuilder.AddColumn<string>(
                name: "TrailerUrl",
                table: "Soundtrack",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
