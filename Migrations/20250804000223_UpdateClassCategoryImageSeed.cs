using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Equinox.Migrations
{
    /// <inheritdoc />
    public partial class UpdateClassCategoryImageSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "ClassCategories",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "ClassCategories",
                keyColumn: "ClassCategoryId",
                keyValue: 1,
                column: "Image",
                value: "boxing.jpg");

            migrationBuilder.UpdateData(
                table: "ClassCategories",
                keyColumn: "ClassCategoryId",
                keyValue: 2,
                column: "Image",
                value: "yoga.jpg");

            migrationBuilder.UpdateData(
                table: "ClassCategories",
                keyColumn: "ClassCategoryId",
                keyValue: 3,
                column: "Image",
                value: "hiit.jpg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "ClassCategories");
        }
    }
}
