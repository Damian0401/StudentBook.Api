using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentBook.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangedStudentClassDeleteBehaviour : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Classes_ClassId",
                schema: "school",
                table: "Students");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Classes_ClassId",
                schema: "school",
                table: "Students",
                column: "ClassId",
                principalSchema: "school",
                principalTable: "Classes",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Classes_ClassId",
                schema: "school",
                table: "Students");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Classes_ClassId",
                schema: "school",
                table: "Students",
                column: "ClassId",
                principalSchema: "school",
                principalTable: "Classes",
                principalColumn: "Id");
        }
    }
}
