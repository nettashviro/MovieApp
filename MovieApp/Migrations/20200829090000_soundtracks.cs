using Microsoft.EntityFrameworkCore.Migrations;

namespace MovieApp.Migrations
{
    public partial class soundtracks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Performer",
                table: "Soundtrack");

            migrationBuilder.DropColumn(
                name: "Writer",
                table: "Soundtrack");

            migrationBuilder.AlterColumn<double>(
                name: "Duration",
                table: "Soundtrack",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "PerformerId",
                table: "Soundtrack",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrailerUrl",
                table: "Soundtrack",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WriterId",
                table: "Soundtrack",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Soundtrack_PerformerId",
                table: "Soundtrack",
                column: "PerformerId");

            migrationBuilder.CreateIndex(
                name: "IX_Soundtrack_WriterId",
                table: "Soundtrack",
                column: "WriterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Soundtrack_Official_PerformerId",
                table: "Soundtrack",
                column: "PerformerId",
                principalTable: "Official",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Soundtrack_Official_WriterId",
                table: "Soundtrack",
                column: "WriterId",
                principalTable: "Official",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Soundtrack_Official_PerformerId",
                table: "Soundtrack");

            migrationBuilder.DropForeignKey(
                name: "FK_Soundtrack_Official_WriterId",
                table: "Soundtrack");

            migrationBuilder.DropIndex(
                name: "IX_Soundtrack_PerformerId",
                table: "Soundtrack");

            migrationBuilder.DropIndex(
                name: "IX_Soundtrack_WriterId",
                table: "Soundtrack");

            migrationBuilder.DropColumn(
                name: "PerformerId",
                table: "Soundtrack");

            migrationBuilder.DropColumn(
                name: "TrailerUrl",
                table: "Soundtrack");

            migrationBuilder.DropColumn(
                name: "WriterId",
                table: "Soundtrack");

            migrationBuilder.AlterColumn<int>(
                name: "Duration",
                table: "Soundtrack",
                type: "int",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AddColumn<string>(
                name: "Performer",
                table: "Soundtrack",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Writer",
                table: "Soundtrack",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
