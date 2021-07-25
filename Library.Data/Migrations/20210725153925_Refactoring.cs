using Microsoft.EntityFrameworkCore.Migrations;

namespace Library.Data.Migrations
{
    public partial class Refactoring : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StaffAssignedTheBook",
                table: "BookCheckouts",
                newName: "StaffThatCheckedOutBook");

            migrationBuilder.RenameColumn(
                name: "ErrorMessage",
                table: "BookCheckouts",
                newName: "StaffThatCheckedInBook");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StaffThatCheckedOutBook",
                table: "BookCheckouts",
                newName: "StaffAssignedTheBook");

            migrationBuilder.RenameColumn(
                name: "StaffThatCheckedInBook",
                table: "BookCheckouts",
                newName: "ErrorMessage");
        }
    }
}
