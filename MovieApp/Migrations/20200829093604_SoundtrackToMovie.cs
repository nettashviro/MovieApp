using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieApp.Migrations
{
    public partial class SoundtrackToMovie : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Soundtrack_Movie_MovieId",
                table: "Soundtrack");

            migrationBuilder.DropIndex(
                name: "IX_Soundtrack_MovieId",
                table: "Soundtrack");

            migrationBuilder.DropColumn(
                name: "MovieId",
                table: "Soundtrack");

            migrationBuilder.CreateTable(
                name: "SoundtrackOfMovie",
                columns: table => new
                {
                    MovieId = table.Column<int>(nullable: false),
                    SoundtrackId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoundtrackOfMovie", x => new { x.MovieId, x.SoundtrackId });
                    table.ForeignKey(
                        name: "FK_SoundtrackOfMovie_Movie_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SoundtrackOfMovie_Soundtrack_SoundtrackId",
                        column: x => x.SoundtrackId,
                        principalTable: "Soundtrack",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SoundtrackOfMovie_SoundtrackId",
                table: "SoundtrackOfMovie",
                column: "SoundtrackId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SoundtrackOfMovie");

            migrationBuilder.AddColumn<int>(
                name: "MovieId",
                table: "Soundtrack",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Soundtrack_MovieId",
                table: "Soundtrack",
                column: "MovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_Soundtrack_Movie_MovieId",
                table: "Soundtrack",
                column: "MovieId",
                principalTable: "Movie",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
