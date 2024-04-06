using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Added_Alternative_key_for_CoursesStudents_And_restoredFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_CoursesStudents_StudentId_CourseId",
                table: "CoursesStudents",
                columns: new[] { "StudentId", "CourseId" });

            migrationBuilder.CreateIndex(
                name: "IX_CoursesStudents_CourseId",
                table: "CoursesStudents",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_CoursesStudents_Courses_CourseId",
                table: "CoursesStudents",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CoursesStudents_Students_StudentId",
                table: "CoursesStudents",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoursesStudents_Courses_CourseId",
                table: "CoursesStudents");

            migrationBuilder.DropForeignKey(
                name: "FK_CoursesStudents_Students_StudentId",
                table: "CoursesStudents");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_CoursesStudents_StudentId_CourseId",
                table: "CoursesStudents");

            migrationBuilder.DropIndex(
                name: "IX_CoursesStudents_CourseId",
                table: "CoursesStudents");
        }
    }
}
