using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieApp.Migrations
{
    public partial class OfficialOfMovie : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movie_Official_Id_Official",
                table: "Movie");

            migrationBuilder.DropIndex(
                name: "IX_Movie_Id_Official",
                table: "Movie");

            migrationBuilder.DropColumn(
                name: "Id_Official",
                table: "Movie");

            migrationBuilder.CreateTable(
                name: "OfficialOfMovie",
                columns: table => new
                {
                    MovieId = table.Column<int>(nullable: false),
                    OfficialId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfficialOfMovie", x => new { x.MovieId, x.OfficialId });
                    table.ForeignKey(
                        name: "FK_OfficialOfMovie_Movie_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OfficialOfMovie_Official_OfficialId",
                        column: x => x.OfficialId,
                        principalTable: "Official",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OfficialOfMovie_OfficialId",
                table: "OfficialOfMovie",
                column: "OfficialId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OfficialOfMovie");

            migrationBuilder.AddColumn<int>(
                name: "Id_Official",
                table: "Movie",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Movie_Id_Official",
                table: "Movie",
                column: "Id_Official");

            migrationBuilder.AddForeignKey(
                name: "FK_Movie_Official_Id_Official",
                table: "Movie",
                column: "Id_Official",
                principalTable: "Official",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
