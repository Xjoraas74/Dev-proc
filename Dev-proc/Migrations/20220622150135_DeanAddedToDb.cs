using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dev_proc.Migrations
{
    public partial class DeanAddedToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dean_AspNetUsers_UserId",
                table: "Dean");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Dean",
                table: "Dean");

            migrationBuilder.RenameTable(
                name: "Dean",
                newName: "Deans");

            migrationBuilder.RenameIndex(
                name: "IX_Dean_UserId",
                table: "Deans",
                newName: "IX_Deans_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Deans",
                table: "Deans",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Deans_AspNetUsers_UserId",
                table: "Deans",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deans_AspNetUsers_UserId",
                table: "Deans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Deans",
                table: "Deans");

            migrationBuilder.RenameTable(
                name: "Deans",
                newName: "Dean");

            migrationBuilder.RenameIndex(
                name: "IX_Deans_UserId",
                table: "Dean",
                newName: "IX_Dean_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Dean",
                table: "Dean",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Dean_AspNetUsers_UserId",
                table: "Dean",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
