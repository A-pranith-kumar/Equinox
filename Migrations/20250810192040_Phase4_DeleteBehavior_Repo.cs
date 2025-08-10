using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Equinox.Migrations
{
    /// <inheritdoc />
    public partial class Phase4_DeleteBehavior_Repo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquinoxClasses_ClassCategories_ClassCategoryId",
                table: "EquinoxClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_EquinoxClasses_Clubs_ClubId",
                table: "EquinoxClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_EquinoxClasses_Users_CoachId",
                table: "EquinoxClasses");

            migrationBuilder.AddForeignKey(
                name: "FK_EquinoxClasses_ClassCategories_ClassCategoryId",
                table: "EquinoxClasses",
                column: "ClassCategoryId",
                principalTable: "ClassCategories",
                principalColumn: "ClassCategoryId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EquinoxClasses_Clubs_ClubId",
                table: "EquinoxClasses",
                column: "ClubId",
                principalTable: "Clubs",
                principalColumn: "ClubId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EquinoxClasses_Users_CoachId",
                table: "EquinoxClasses",
                column: "CoachId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquinoxClasses_ClassCategories_ClassCategoryId",
                table: "EquinoxClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_EquinoxClasses_Clubs_ClubId",
                table: "EquinoxClasses");

            migrationBuilder.DropForeignKey(
                name: "FK_EquinoxClasses_Users_CoachId",
                table: "EquinoxClasses");

            migrationBuilder.AddForeignKey(
                name: "FK_EquinoxClasses_ClassCategories_ClassCategoryId",
                table: "EquinoxClasses",
                column: "ClassCategoryId",
                principalTable: "ClassCategories",
                principalColumn: "ClassCategoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EquinoxClasses_Clubs_ClubId",
                table: "EquinoxClasses",
                column: "ClubId",
                principalTable: "Clubs",
                principalColumn: "ClubId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EquinoxClasses_Users_CoachId",
                table: "EquinoxClasses",
                column: "CoachId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
