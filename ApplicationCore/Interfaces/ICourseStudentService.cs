using ApplicationCore.DTOs;

namespace ApplicationCore.Interfaces
{
	public interface ICourseStudentService
	{
		void AddCourseStudent(CourseStudentDto courseStudent);
		void RemoveCourseStudent(int courseStudentId);
		void UpdateCourseStudent(int courseStudentId, CourseStudentDto courseStudent);
		IEnumerable<CourseStudentDto> GetAllCourseStudent();
		CourseStudentDto? GetCourseStudentById(int courseStudentId);
	}
}
