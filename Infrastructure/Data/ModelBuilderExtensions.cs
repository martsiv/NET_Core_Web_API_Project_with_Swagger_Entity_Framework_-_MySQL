using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
	public static class ModelBuilderExtensions
	{
		public static void SeedData(this ModelBuilder builder)
		{
			builder.Entity<Teacher>().HasData(new[]
			   {
				new Teacher { Id = 1, Name = "John", Surname = "Doe", BirthDate = new DateTime(1975, 5, 10), WorkExperienceFullYears = 10 },
				new Teacher { Id = 2, Name = "Emily", Surname = "Smith", BirthDate = new DateTime(1982, 8, 25), WorkExperienceFullYears = 8 }
            });
			builder.Entity<Student>().HasData(new[]
			{
				new Student { Id = 1, Name = "Alice", Surname = "Johnson", BirthDate = new DateTime(2000, 3, 15), GroupName = "Group A" },
				new Student { Id = 2, Name = "Michael", Surname = "Brown", BirthDate = new DateTime(1998, 9, 20), GroupName = "Group B" },
				new Student { Id = 3, Name = "Sophia", Surname = "Martinez", BirthDate = new DateTime(2001, 12, 5), GroupName = "Group A" },
				new Student { Id = 4, Name = "Emma", Surname = "Wilson", BirthDate = new DateTime(1999, 6, 10), GroupName = "Group B" },
				new Student { Id = 5, Name = "James", Surname = "Taylor", BirthDate = new DateTime(2002, 2, 25), GroupName = "Group A" }
			});
			builder.Entity<Course>().HasData(new[]
			{
				new Course { Id = 1, Name = "Introduction to Programming", TeacherId = 1 },
				new Course { Id = 2, Name = "Web Development Fundamentals", TeacherId = 2 },
				new Course { Id = 3, Name = "Database Design and Management", TeacherId = 1 }
            });
			builder.Entity<CourseStudent>().HasData(new[]
			{
				new CourseStudent { Id = 1, CourseId = 1, StudentId = 1 },
				new CourseStudent { Id = 2, CourseId = 1, StudentId = 2 },
				new CourseStudent { Id = 3, CourseId = 2, StudentId = 2 },
				new CourseStudent { Id = 4, CourseId = 3, StudentId = 3 },
				new CourseStudent { Id = 5, CourseId = 2, StudentId = 4 },
				new CourseStudent { Id = 6, CourseId = 3, StudentId = 5 }
			});
		}
	}
}
