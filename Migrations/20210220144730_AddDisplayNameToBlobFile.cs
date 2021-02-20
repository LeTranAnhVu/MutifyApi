using Microsoft.EntityFrameworkCore.Migrations;

namespace Mutify.Migrations
{
    public partial class AddDisplayNameToBlobFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "BlobFiles",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "BlobFiles");
        }
    }
}
