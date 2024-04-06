using ApplicationCore.DTOs;

namespace ApplicationCore.Interfaces
{
	public interface IStudentService
	{
		void AddStudent(StudentDto student);
		void RemoveStudent(int studentId);
		void UpdateStudent(int studentId, StudentDto student);
		IEnumerable<StudentDto> GetAllStudents();
		IEnumerable<StudentDto> GetStudentsByCourse(int courseId);
		StudentDto? GetStudentById(int studentId);
	}
}
