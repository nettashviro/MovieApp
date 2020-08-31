using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieApp.Migrations
{
    public partial class ini : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(nullable: false),
                    Username = table.Column<string>(nullable: false),
                    Password = table.Column<string>(maxLength: 100, nullable: false),
                    Type = table.Column<int>(nullable: false),
                    ProfileImageUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Official",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Role = table.Column<int>(nullable: false),
                    Gender = table.Column<int>(nullable: false),
                    Birthdate = table.Column<DateTime>(nullable: false),
                    OriginCountry = table.Column<string>(nullable: true),
                    ImageUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Official", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tweet",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TweetId = table.Column<long>(nullable: false),
                    Author = table.Column<string>(nullable: true),
                    MovieId = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tweet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Movie",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    Language = table.Column<string>(nullable: true),
                    Year = table.Column<int>(nullable: false),
                    Genre = table.Column<int>(nullable: false),
                    Duration = table.Column<int>(nullable: false),
                    TrailerUrl = table.Column<string>(nullable: true),
                    Rating = table.Column<double>(nullable: false),
                    ImageUrl = table.Column<string>(nullable: false),
                    MovieIdInTMDB = table.Column<int>(nullable: false),
                    MovieClickedId = table.Column<int>(nullable: true),
                    MovieWatchedId = table.Column<int>(nullable: true),
                    MovieWatchlistId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movie", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Movie_Account_MovieClickedId",
                        column: x => x.MovieClickedId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Movie_Account_MovieWatchedId",
                        column: x => x.MovieWatchedId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Movie_Account_MovieWatchlistId",
                        column: x => x.MovieWatchlistId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Soundtrack",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Duration = table.Column<double>(nullable: false),
                    SoundtrackUrl = table.Column<string>(nullable: true),
                    WriterId = table.Column<int>(nullable: true),
                    PerformerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Soundtrack", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Soundtrack_Official_PerformerId",
                        column: x => x.PerformerId,
                        principalTable: "Official",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Soundtrack_Official_WriterId",
                        column: x => x.WriterId,
                        principalTable: "Official",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_OfficialOfMovie_OfficialId",
                table: "OfficialOfMovie",
                column: "OfficialId");

            migrationBuilder.CreateIndex(
                name: "IX_Soundtrack_PerformerId",
                table: "Soundtrack",
                column: "PerformerId");

            migrationBuilder.CreateIndex(
                name: "IX_Soundtrack_WriterId",
                table: "Soundtrack",
                column: "WriterId");

            migrationBuilder.CreateIndex(
                name: "IX_SoundtrackOfMovie_SoundtrackId",
                table: "SoundtrackOfMovie",
                column: "SoundtrackId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OfficialOfMovie");

            migrationBuilder.DropTable(
                name: "SoundtrackOfMovie");

            migrationBuilder.DropTable(
                name: "Tweet");

            migrationBuilder.DropTable(
                name: "Movie");

            migrationBuilder.DropTable(
                name: "Soundtrack");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "Official");
        }
    }
}
