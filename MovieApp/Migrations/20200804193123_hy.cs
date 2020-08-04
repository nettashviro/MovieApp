using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieApp.Migrations
{
    public partial class hy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id_Director",
                table: "Movie",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "Movie",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Director",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Gender = table.Column<int>(nullable: false),
                    Birthdate = table.Column<DateTime>(nullable: false),
                    OriginCountry = table.Column<string>(nullable: true)
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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "Language",
                table: "Movie");
        }
    }
}
