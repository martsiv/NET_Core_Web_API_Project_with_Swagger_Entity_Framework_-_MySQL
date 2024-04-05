using ApplicationCore;
using ApplicationCore.DTOs;
using ApplicationCore.Interfaces;
using AutoMapper;
using Infrastructure;

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

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			// auto mapper configuration for Business logic
			builder.Services.AddAutoMapper();

			// add custom servies from Business logic
			builder.Services.AddCustomServices();

			var app = builder.Build();

			// Create logger
			ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
			ILogger logger = loggerFactory.CreateLogger<Program>();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();

			// Students
			app.MapGet("/api/students/", async (IStudentService studentService) =>
			{
				var students = studentService.GetAllStudents();
				return Results.Json(new { students });
			});
			app.MapGet("/api/students/{id:int}", async (int id, IStudentService studentService) =>
			{
				var student = studentService.GetStudentById(id);
				if (student == null)
					return Results.NotFound(new { message = "Student not found" });
				return Results.Json(student);
			});
			app.MapPost("/api/students/", async (CreateStudentDto student, IStudentService studentService, IMapper mapper) =>
			{
				studentService.AddStudent(mapper.Map<StudentDto>(student));
				return student;
			});
			app.MapPut("/api/students", async (StudentDto studentDto, IStudentService studentService) =>
			{
				var student = studentService.GetStudentById(studentDto.Id);
				if (student == null)
					return Results.NotFound(new { message = "Student not found" });
				studentService.UpdateStudent(student.Id, studentDto);
				return Results.Json(studentDto);
			});
			app.MapDelete("/api/students/{id:int}", async (int id, IStudentService studentService) =>
			{
				var student = studentService.GetStudentById(id);
				if (student == null)
					return Results.NotFound(new { message = "Student not found" });
				studentService.RemoveStudent(id);
				return Results.Json(student);
			});

			// Teachers 
			app.MapGet("/api/teachers/", async (ITeacherService teacherService) =>
			{
				var teachers = teacherService.GetAllTeachers();
				return Results.Json(new { teachers });
			});
			app.MapGet("/api/teachers/{id:int}", async (int id, ITeacherService teacherService) =>
			{
				var teacher = teacherService.GetTeacherById(id);
				if (teacher == null)
					return Results.NotFound(new { message = "Teacher not found" });
				return Results.Json(teacher);
			});
			app.MapPost("/api/teachers/", async (CreateTeacherDto teacher, ITeacherService teacherService, IMapper mapper) =>
			{
				teacherService.AddTeacher(mapper.Map<TeacherDto>(teacher));
				return teacher;
			});
			app.MapPut("/api/teachers", async (TeacherDto teacherDto, ITeacherService teacherService) =>
			{
				var teacher = teacherService.GetTeacherById(teacherDto.Id);
				if (teacher == null)
					return Results.NotFound(new { message = "Teacher not found" });
				teacherService.UpdateTeacher(teacher.Id, teacherDto);
				return Results.Json(teacherDto);
			});
			app.MapDelete("/api/teachers/{id:int}", async (int id, ITeacherService teacherService) =>
			{
				var teacher = teacherService.GetTeacherById(id);
				if (teacher == null)
					return Results.NotFound(new { message = "Teacher not found" });
				teacherService.RemoveTeacher(id);
				return Results.Json(teacher);
			});

			// Courses
			app.MapGet("/api/courses/", async (ICourseService courseService) =>
			{
				var courses = courseService.GetAllCourses();
				return Results.Json(new { courses });
			});
			app.MapGet("/api/courses/{id:int}", async (int id, ICourseService courseService) =>
			{
				var course = courseService.GetCourseById(id);
				if (course == null)
					return Results.NotFound(new { message = "Course not found" });
				return Results.Json(course);
			});
			app.MapPost("/api/courses/", async (CreateCourseDto course, ICourseService courseService, IMapper mapper) =>
			{
				courseService.AddCourse(mapper.Map<CourseDto>(course));
				return course;
			});
			app.MapPut("/api/courses", async (CourseDto courseDto, ICourseService courseService) =>
			{
				var course = courseService.GetCourseById(courseDto.Id);
				if (course == null)
					return Results.NotFound(new { message = "Course not found" });
				courseService.UpdateCourse(course.Id, courseDto);
				return Results.Json(courseDto);
			});
			app.MapDelete("/api/courses/{id:int}", async (int id, ICourseService courseService) =>
			{
				var course = courseService.GetCourseById(id);
				if (course == null) 
					return Results.NotFound(new { message = "Course not found" });
				courseService.RemoveCourse(id);
				return Results.Json(course);
			});

			app.Run();
		}
	}
}
