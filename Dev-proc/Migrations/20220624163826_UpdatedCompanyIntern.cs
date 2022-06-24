using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dev_proc.Migrations
{
    public partial class UpdatedCompanyIntern : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StudentCompanyInterns_CompanyId",
                table: "StudentCompanyInterns");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCompanyInterns_CompanyId",
                table: "StudentCompanyInterns",
                column: "CompanyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StudentCompanyInterns_CompanyId",
                table: "StudentCompanyInterns");

            migrationBuilder.CreateIndex(
                name: "IX_StudentCompanyInterns_CompanyId",
                table: "StudentCompanyInterns",
                column: "CompanyId",
                unique: true);
        }
    }
}
