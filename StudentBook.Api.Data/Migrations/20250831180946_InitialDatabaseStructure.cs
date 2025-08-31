using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentBook.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialDatabaseStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "school");

            migrationBuilder.CreateTable(
                name: "LeadingTeachers",
                schema: "school",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeadingTeachers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Classes",
                schema: "school",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    LeadingTeacherId = table.Column<long>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Classes_LeadingTeachers_LeadingTeacherId",
                        column: x => x.LeadingTeacherId,
                        principalSchema: "school",
                        principalTable: "LeadingTeachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                schema: "school",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Identifier = table.Column<string>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    City = table.Column<string>(type: "TEXT", nullable: true),
                    Street = table.Column<string>(type: "TEXT", nullable: true),
                    PostalCode = table.Column<string>(type: "TEXT", nullable: true),
                    ClassId = table.Column<Guid>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Students_Classes_ClassId",
                        column: x => x.ClassId,
                        principalSchema: "school",
                        principalTable: "Classes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Classes_LeadingTeacherId",
                schema: "school",
                table: "Classes",
                column: "LeadingTeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_LeadingTeachers_Name",
                schema: "school",
                table: "LeadingTeachers",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_ClassId",
                schema: "school",
                table: "Students",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_Identifier",
                schema: "school",
                table: "Students",
                column: "Identifier",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Students",
                schema: "school");

            migrationBuilder.DropTable(
                name: "Classes",
                schema: "school");

            migrationBuilder.DropTable(
                name: "LeadingTeachers",
                schema: "school");
        }
    }
}
