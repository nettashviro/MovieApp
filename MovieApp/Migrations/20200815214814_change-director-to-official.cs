using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieApp.Migrations
{
    public partial class changedirectortoofficial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movie_Director_Id_Director",
                table: "Movie");

            migrationBuilder.DropTable(
                name: "Director");

            migrationBuilder.DropIndex(
                name: "IX_Movie_Id_Director",
                table: "Movie");

            migrationBuilder.DropColumn(
                name: "Id_Director",
                table: "Movie");

            migrationBuilder.AddColumn<int>(
                name: "Id_Official",
                table: "Movie",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Official",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Role = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Gender = table.Column<int>(nullable: false),
                    Birthdate = table.Column<DateTime>(nullable: false),
                    OriginCountry = table.Column<string>(nullable: true),
                    ImageUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Official", x => x.Id);
                });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movie_Official_Id_Official",
                table: "Movie");

            migrationBuilder.DropTable(
                name: "Official");

            migrationBuilder.DropIndex(
                name: "IX_Movie_Id_Official",
                table: "Movie");

            migrationBuilder.DropColumn(
                name: "Id_Official",
                table: "Movie");

            migrationBuilder.AddColumn<int>(
                name: "Id_Director",
                table: "Movie",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Director",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Birthdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginCountry = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Director", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Movie_Id_Director",
                table: "Movie",
                column: "Id_Director");

            migrationBuilder.AddForeignKey(
                name: "FK_Movie_Director_Id_Director",
                table: "Movie",
                column: "Id_Director",
                principalTable: "Director",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
