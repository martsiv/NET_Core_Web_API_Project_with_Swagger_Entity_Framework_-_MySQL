using ApplicationCore;
using ApplicationCore.DTOs;
using ApplicationCore.Interfaces;
using AutoMapper;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using WebApp.Logger;

namespace WebApp
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Connection string from secrets
			builder.Configuration.AddJsonFile("secrets.json", optional: true);
			string connStr = builder.Configuration.GetConnectionString("DefaultConnection")!;

			// Add services to the container.
			builder.Services.AddAuthorization();

			builder.Services.AddDbContext(connStr);
			builder.Services.AddRepositories();

			// Add file logger
			builder.Logging.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt"));

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			// Auto mapper configuration for Business logic
			builder.Services.AddAutoMapper();
			// Fluent validators configuration
			builder.Services.AddFluentValidator();

			// Add custom servies from Business logic
			builder.Services.AddCustomServices();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			// Exeption handler
			app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

			app.UseAuthorization();

			// Students
			app.MapGet("/api/students/", async (IStudentService studentService) =>
			{
				var students = studentService.GetAllStudents();
				return Results.Json(students);
			});
			app.MapGet("/api/students/{id:int}", async (int id, IStudentService studentService) =>
			{
				var student = studentService.GetStudentById(id);
				return Results.Json(student);
			});
			app.MapPost("/api/students/", async ([FromBody] CreateStudentDto student, IStudentService studentService) =>
			{
				studentService.AddStudent(student);
				return Results.Ok();
			});
			app.MapPut("/api/students", async ([FromBody] StudentDto studentDto, IStudentService studentService) =>
			{
				var student = studentService.GetStudentById(studentDto.Id);
				studentService.UpdateStudent(student.Id, studentDto);
				return Results.Json(studentDto);
			});
			app.MapDelete("/api/students/{id:int}", async (int id, IStudentService studentService) =>
			{
				var student = studentService.GetStudentById(id);
				studentService.RemoveStudent(id);
				return Results.Json(student);
			});

			// Teachers 
			app.MapGet("/api/teachers/", async (ITeacherService teacherService) =>
			{
				var teachers = teacherService.GetAllTeachers();
				return Results.Json(teachers);
			});
			app.MapGet("/api/teachers/{id:int}", async (int id, ITeacherService teacherService) =>
			{
				var teacher = teacherService.GetTeacherById(id);
				return Results.Json(teacher);
			});
			app.MapPost("/api/teachers/", async ([FromBody] CreateTeacherDto teacher, ITeacherService teacherService) =>
			{
				teacherService.AddTeacher(teacher);
				return Results.Ok();
			});
			app.MapPut("/api/teachers", async ([FromBody] TeacherDto teacherDto, ITeacherService teacherService) =>
			{
				var teacher = teacherService.GetTeacherById(teacherDto.Id);
				teacherService.UpdateTeacher(teacher.Id, teacherDto);
				return Results.Json(teacherDto);
			});
			app.MapDelete("/api/teachers/{id:int}", async (int id, ITeacherService teacherService) =>
			{
				var teacher = teacherService.GetTeacherById(id);
				teacherService.RemoveTeacher(id);
				return Results.Json(teacher);
			});

			// Courses
			app.MapGet("/api/courses/", async (ICourseService courseService) =>
			{
				var courses = courseService.GetAllCourses();
				return Results.Json(courses);
			});
			app.MapGet("/api/courses/{id:int}", async (int id, ICourseService courseService) =>
			{
				var course = courseService.GetCourseById(id);
				return Results.Json(course);
			});
			app.MapPost("/api/courses/", async ([FromBody] CreateCourseDto course, ICourseService courseService) =>
			{
				courseService.AddCourse(course);
				return Results.Ok();
			});
			app.MapPut("/api/courses", async ([FromBody] CourseDto courseDto, ICourseService courseService) =>
			{
				var course = courseService.GetCourseById(courseDto.Id);
				courseService.UpdateCourse(course.Id, courseDto);
				return Results.Json(courseDto);
			});
			app.MapDelete("/api/courses/{id:int}", async (int id, ICourseService courseService) =>
			{
				var course = courseService.GetCourseById(id);
				courseService.RemoveCourse(id);
				return Results.Json(course);
			});

			// Course-Students
			app.MapGet("/api/courses/{courseId:int}/students", async (int courseId, IStudentService studentService) =>
			{
				var students = studentService.GetStudentsByCourse(courseId);
				if (students.Count() == 0)
					return Results.NotFound(new { message = "No items were found matching the given search criteria." });
				return Results.Json(students);
			});

			app.MapGet("/api/students/{studentId:int}/courses", async (int studentId, ICourseService courseService) =>
			{
				var courses = courseService.GetCoursesByStudent(studentId);
				if (courses.Count() == 0)
					return Results.NotFound(new { message = "No items were found matching the given search criteria." });
				return Results.Json(courses);
			});

			app.MapGet("/api/teacher/{teacherId:int}/courses", async (int teacherId, ICourseService courseService) =>
			{
				var courses = courseService.GetCoursesByTeacher(teacherId);
				if (courses.Count() == 0)
					return Results.NotFound(new { message = "No items were found matching the given search criteria." });
				return Results.Json(courses);
			});

			app.MapGet("/api/courses/{courseId:int}/students/{studentId:int}", async ([FromRoute] int courseId, [FromRoute] int studentId, [FromServices] ICourseStudentService courseStudentService) =>
			{
				var entity = courseStudentService.GetCourseStudentByIds(courseId, studentId);
				return Results.Json(entity);
			});

			app.MapPost("/api/courses/{courseId:int}/students/{studentId:int}", async ([FromRoute] int courseId, [FromRoute] int studentId, [FromServices] ICourseStudentService courseStudentService, IMapper mapper) =>
			{
				var courseStudent = new CourseStudentDto() { CourseId = courseId, StudentId = studentId };
				courseStudentService.AddCourseStudent(courseStudent);
				return Results.Ok();
			});

			app.MapDelete("/api/courses/{courseId:int}/students/{studentId:int}", async ([FromRoute] int courseId, [FromRoute] int studentId, [FromServices] ICourseStudentService courseStudentService) =>
			{
				var courseStudent = courseStudentService.GetCourseStudentByIds(courseId, studentId);
				courseStudentService.RemoveCourseStudent(courseStudent.Id);
				return Results.Json(courseStudent);
			});

			app.Run();
		}
	}
}
