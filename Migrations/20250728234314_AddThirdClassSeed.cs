using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Equinox.Migrations
{
    /// <inheritdoc />
    public partial class AddThirdClassSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "EquinoxClasses",
                columns: new[] { "EquinoxClassId", "ClassCategoryId", "ClassDay", "ClassPicture", "ClubId", "CoachId", "Name", "Time" },
                values: new object[] { 3, 3, "Friday", "hiitjunior.jpg", 3, 1, "HIIT Junior", "5 PM – 6 PM" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EquinoxClasses",
                keyColumn: "EquinoxClassId",
                keyValue: 3);
        }
    }
}
