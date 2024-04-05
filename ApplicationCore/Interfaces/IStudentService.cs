using ApplicationCore.DTOs;

namespace ApplicationCore.Interfaces
{
	public interface IStudentService
	{
		void AddStudent(StudentDto student);
		void RemoveStudent(int studentId);
		void UpdateStudent(int studentId, StudentDto student);
		IEnumerable<StudentDto> GetAllStudents();
		StudentDto? GetStudentById(int studentId);
	}
}
