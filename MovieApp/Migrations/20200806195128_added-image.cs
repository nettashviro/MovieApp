using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieApp.Migrations
{
    public partial class addedimage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Director");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Movie",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateTable(
                name: "MovieReview",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rank = table.Column<int>(nullable: false),
                    Title = table.Column<string>(maxLength: 30, nullable: true),
                    Description = table.Column<string>(maxLength: 256, nullable: true),
                    IsViolent = table.Column<bool>(nullable: false),
                    IsBlackAndWhite = table.Column<bool>(nullable: false),
                    RecommendedAge = table.Column<int>(nullable: false),
                    IsHavingSequel = table.Column<bool>(nullable: false),
                    movie_id = table.Column<int>(nullable: true)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovieReview");

            migrationBuilder.AlterColumn<long>(
                name: "Name",
                table: "Movie",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Director",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
