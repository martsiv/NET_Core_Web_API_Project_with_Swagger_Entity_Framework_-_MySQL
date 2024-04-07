using ApplicationCore.DTOs;

namespace ApplicationCore.Interfaces
{
	public interface ITeacherService
	{
		void AddTeacher(CreateTeacherDto teacher);
		void RemoveTeacher(int teacherId);
		void UpdateTeacher(int teacherId, TeacherDto teacher);
		IEnumerable<TeacherViewDto> GetAllTeachers();
		TeacherViewDto GetTeacherById(int teacherId);
		Task AddTeacherAsync(CreateTeacherDto teacher);
		Task RemoveTeacherAsync(int teacherId);
		Task UpdateTeacherAsync(int teacherId, TeacherDto teacher);
		Task<IEnumerable<TeacherViewDto>> GetAllTeachersAsync();
		Task<TeacherViewDto> GetTeacherByIdAsync(int teacherId);

	}
}
