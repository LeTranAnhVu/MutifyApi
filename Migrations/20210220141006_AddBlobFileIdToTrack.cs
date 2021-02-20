using Microsoft.EntityFrameworkCore.Migrations;

namespace Mutify.Migrations
{
    public partial class AddBlobFileIdToTrack : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BlobFileId",
                table: "Tracks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tracks_BlobFileId",
                table: "Tracks",
                column: "BlobFileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tracks_BlobFiles_BlobFileId",
                table: "Tracks",
                column: "BlobFileId",
                principalTable: "BlobFiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tracks_BlobFiles_BlobFileId",
                table: "Tracks");

            migrationBuilder.DropIndex(
                name: "IX_Tracks_BlobFileId",
                table: "Tracks");

            migrationBuilder.DropColumn(
                name: "BlobFileId",
                table: "Tracks");
        }
    }
}
