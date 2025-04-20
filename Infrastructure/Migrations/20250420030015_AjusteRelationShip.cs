using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AjusteRelationShip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Teachers_PictureId",
                table: "Teachers");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_PictureId",
                table: "Teachers",
                column: "PictureId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Teachers_PictureId",
                table: "Teachers");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_PictureId",
                table: "Teachers",
                column: "PictureId");
        }
    }
}
