using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTemporaryPath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Course_Category",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Course_Picture",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Course_Trailer_Video",
                table: "Courses");

            migrationBuilder.AlterColumn<string>(
                name: "AwsKey",
                table: "Videos",
                type: "varchar",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<string>(
                name: "TemporaryPath",
                table: "Videos",
                type: "varchar",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AwsKey",
                table: "Pictures",
                type: "varchar",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<string>(
                name: "TemporaryPath",
                table: "Pictures",
                type: "varchar",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Course_Category",
                table: "Courses",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Course_Picture",
                table: "Courses",
                column: "PictureId",
                principalTable: "Pictures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Course_Trailer_Video",
                table: "Courses",
                column: "TrailerId",
                principalTable: "Videos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Course_Category",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Course_Picture",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Course_Trailer_Video",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "TemporaryPath",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "TemporaryPath",
                table: "Pictures");

            migrationBuilder.AlterColumn<string>(
                name: "AwsKey",
                table: "Videos",
                type: "varchar",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AwsKey",
                table: "Pictures",
                type: "varchar",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Course_Category",
                table: "Courses",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Course_Picture",
                table: "Courses",
                column: "PictureId",
                principalTable: "Pictures",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Course_Trailer_Video",
                table: "Courses",
                column: "TrailerId",
                principalTable: "Videos",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
