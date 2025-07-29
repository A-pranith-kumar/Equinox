using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Equinox.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClassCategories",
                columns: table => new
                {
                    ClassCategoryId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassCategories", x => x.ClassCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Clubs",
                columns: table => new
                {
                    ClubId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clubs", x => x.ClubId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    DOB = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsCoach = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "EquinoxClasses",
                columns: table => new
                {
                    EquinoxClassId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    ClassPicture = table.Column<string>(type: "TEXT", nullable: false),
                    ClassDay = table.Column<string>(type: "TEXT", nullable: false),
                    Time = table.Column<string>(type: "TEXT", nullable: false),
                    ClassCategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    CoachId = table.Column<int>(type: "INTEGER", nullable: false),
                    ClubId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquinoxClasses", x => x.EquinoxClassId);
                    table.ForeignKey(
                        name: "FK_EquinoxClasses_ClassCategories_ClassCategoryId",
                        column: x => x.ClassCategoryId,
                        principalTable: "ClassCategories",
                        principalColumn: "ClassCategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquinoxClasses_Clubs_ClubId",
                        column: x => x.ClubId,
                        principalTable: "Clubs",
                        principalColumn: "ClubId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EquinoxClasses_Users_CoachId",
                        column: x => x.CoachId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    BookingId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EquinoxClassId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.BookingId);
                    table.ForeignKey(
                        name: "FK_Bookings_EquinoxClasses_EquinoxClassId",
                        column: x => x.EquinoxClassId,
                        principalTable: "EquinoxClasses",
                        principalColumn: "EquinoxClassId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ClassCategories",
                columns: new[] { "ClassCategoryId", "Name" },
                values: new object[,]
                {
                    { 1, "Boxing" },
                    { 2, "Yoga" },
                    { 3, "HIIT" }
                });

            migrationBuilder.InsertData(
                table: "Clubs",
                columns: new[] { "ClubId", "Name", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, "Chicago Loop", "312-000-0001" },
                    { 2, "West Chicago", "312-000-0002" },
                    { 3, "Lincoln Park", "312-000-0003" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "DOB", "Email", "IsCoach", "Name", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, new DateTime(1985, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "mike@equinox.com", true, "Coach Mike", "555-1111" },
                    { 2, new DateTime(1990, 3, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "lisa@equinox.com", true, "Coach Lisa", "555-2222" }
                });

            migrationBuilder.InsertData(
                table: "EquinoxClasses",
                columns: new[] { "EquinoxClassId", "ClassCategoryId", "ClassDay", "ClassPicture", "ClubId", "CoachId", "Name", "Time" },
                values: new object[,]
                {
                    { 1, 1, "Monday", "boxing101.jpg", 1, 1, "Boxing 101", "8 AM – 9 AM" },
                    { 2, 2, "Wednesday", "hatha.jpg", 2, 2, "Hatha Yoga", "6 PM – 7 PM" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_EquinoxClassId",
                table: "Bookings",
                column: "EquinoxClassId");

            migrationBuilder.CreateIndex(
                name: "IX_EquinoxClasses_ClassCategoryId",
                table: "EquinoxClasses",
                column: "ClassCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_EquinoxClasses_ClubId",
                table: "EquinoxClasses",
                column: "ClubId");

            migrationBuilder.CreateIndex(
                name: "IX_EquinoxClasses_CoachId",
                table: "EquinoxClasses",
                column: "CoachId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "EquinoxClasses");

            migrationBuilder.DropTable(
                name: "ClassCategories");

            migrationBuilder.DropTable(
                name: "Clubs");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
