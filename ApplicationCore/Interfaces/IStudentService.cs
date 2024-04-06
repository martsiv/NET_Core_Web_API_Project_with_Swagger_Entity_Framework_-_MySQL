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
		StudentViewDto? GetStudentById(int studentId);
	}
}
