using ApplicationCore.DTOs;

namespace ApplicationCore.Interfaces
{
	public interface ICourseService
	{
		void AddCourse(CourseDto course);
		void RemoveCourse(int courseId);
		void UpdateCourse(int courseId, CourseDto course);
		IEnumerable<CourseDto> GetAllCourses();
		CourseDto? GetCourseById(int courseId);
	}
}
