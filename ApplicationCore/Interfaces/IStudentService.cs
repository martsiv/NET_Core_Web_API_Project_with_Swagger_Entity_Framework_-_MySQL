using ApplicationCore.DTOs;

namespace ApplicationCore.Interfaces
{
	public interface IStudentService
	{
		void AddStudent(CreateStudentDto student);
		void RemoveStudent(int studentId);
		void UpdateStudent(int studentId, StudentDto student);
		IEnumerable<StudentViewDto> GetAllStudents();
		IEnumerable<StudentDto> GetStudentsByCourse(int courseId);
		StudentViewDto GetStudentById(int studentId);
		Task AddStudentAsync(CreateStudentDto student);
		Task RemoveStudentAsync(int studentId);
		Task UpdateStudentAsync(int studentId, StudentDto student);
		Task<IEnumerable<StudentViewDto>> GetAllStudentsAsync();
		Task<IEnumerable<StudentDto>> GetStudentsByCourseAsync(int courseId);
		Task<StudentViewDto> GetStudentByIdAsync(int studentId);

	}
}
