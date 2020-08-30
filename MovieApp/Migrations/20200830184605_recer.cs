using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieApp.Migrations
{
    public partial class recer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovieReview");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MovieReview",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    IsBlackAndWhite = table.Column<bool>(type: "bit", nullable: false),
                    IsHavingSequel = table.Column<bool>(type: "bit", nullable: false),
                    IsViolent = table.Column<bool>(type: "bit", nullable: false),
                    Rank = table.Column<double>(type: "float", nullable: false),
                    RecommendedAge = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    movie_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieReview", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovieReview_Movie_movie_id",
                        column: x => x.movie_id,
                        principalTable: "Movie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovieReview_movie_id",
                table: "MovieReview",
                column: "movie_id");
        }
    }
}
