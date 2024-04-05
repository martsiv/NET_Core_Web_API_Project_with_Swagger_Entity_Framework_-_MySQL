using ApplicationCore.DTOs;

namespace ApplicationCore.Interfaces
{
	public interface ITeacherService
	{
		void AddTeacher(TeacherDto teacher);
		void RemoveTeacher(int teacherId);
		void UpdateTeacher(int teacherId, TeacherDto teacher);
		IEnumerable<TeacherDto> GetAllTeachers();
		TeacherDto? GetTeacherById(int teacherId);
	}
}
