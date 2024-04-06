using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Alternative_key_for_CoursesStudents_and_removerFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoursesStudents_Courses_CourseId",
                table: "CoursesStudents");

            migrationBuilder.DropForeignKey(
                name: "FK_CoursesStudents_Students_StudentId",
                table: "CoursesStudents");

            migrationBuilder.DropIndex(
                name: "IX_CoursesStudents_CourseId",
                table: "CoursesStudents");

            migrationBuilder.DropIndex(
                name: "IX_CoursesStudents_StudentId",
                table: "CoursesStudents");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CoursesStudents_CourseId",
                table: "CoursesStudents",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CoursesStudents_StudentId",
                table: "CoursesStudents",
                column: "StudentId");

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
    }
}
