using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Change.DataAccess.Migrations
{
    public partial class AddSubmitedByToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SubmittedBy",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubmittedBy",
                table: "Requests");
        }
    }
}
