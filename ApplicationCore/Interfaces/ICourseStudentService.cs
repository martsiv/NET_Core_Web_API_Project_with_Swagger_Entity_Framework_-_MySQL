using ApplicationCore.DTOs;

namespace ApplicationCore.Interfaces
{
	public interface ICourseStudentService
	{
		void AddCourseStudent(CourseStudentDto courseStudent);
		void RemoveCourseStudent(int courseStudentId);
		void UpdateCourseStudent(int courseStudentId, CourseStudentDto courseStudent);
		IEnumerable<CourseStudentDto> GetAllCourseStudent();
		CourseStudentDto GetCourseStudentById(int courseStudentId);
		CourseStudentDto GetCourseStudentByIds(int courseId, int studentId);
		Task AddCourseStudentAsync(CourseStudentDto courseStudent);
		Task RemoveCourseStudentAsync(int courseStudentId);
		Task UpdateCourseStudentAsync(int courseStudentId, CourseStudentDto courseStudent);
		Task<IEnumerable<CourseStudentDto>> GetAllCourseStudentAsync();
		Task<CourseStudentDto> GetCourseStudentByIdAsync(int courseStudentId);
		Task<CourseStudentDto> GetCourseStudentByIdsAsync(int courseId, int studentId);
	}
}
