namespace ApplicationCore.DTOs
{
	public class CourseViewDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public TeacherDto? Teacher { get; set; }
		public List<StudentDto> Students { get; set; }
	}
}
