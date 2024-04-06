using ApplicationCore.DTOs;

namespace ApplicationCore.Interfaces
{
	public interface ICourseService
	{
		void AddCourse(CreateCourseDto course);
		void RemoveCourse(int courseId);
		void UpdateCourse(int courseId, CourseDto course);
		IEnumerable<CourseViewDto> GetAllCourses();
		IEnumerable<CourseDto> GetCoursesByStudent(int studentId);
		IEnumerable<CourseDto> GetCoursesByTeacher(int teacherId);
		CourseViewDto? GetCourseById(int courseId);
	}
}
