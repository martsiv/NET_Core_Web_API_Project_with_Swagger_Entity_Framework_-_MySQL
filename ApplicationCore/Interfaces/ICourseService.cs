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
		CourseViewDto GetCourseById(int courseId);
		Task AddCourseAsync(CreateCourseDto course);
		Task RemoveCourseAsync(int courseId);
		Task UpdateCourseAsync(int courseId, CourseDto course);
		Task<IEnumerable<CourseViewDto>> GetAllCoursesAsync();
		Task<IEnumerable<CourseDto>> GetCoursesByStudentAsync(int studentId);
		Task<IEnumerable<CourseDto>> GetCoursesByTeacherAsync(int teacherId);
		Task<CourseViewDto> GetCourseByIdAsync(int courseId);
	}
}
