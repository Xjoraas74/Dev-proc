using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dev_proc.Migrations
{
    public partial class AddedRoleType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "AspNetRoles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "AspNetRoles");
        }
    }
}
