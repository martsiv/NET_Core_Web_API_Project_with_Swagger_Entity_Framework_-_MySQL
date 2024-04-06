namespace ApplicationCore.DTOs
{
	public class StudentViewDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Surname { get; set; }
		public DateTime BirthDate { get; set; }
		public string GroupName { get; set; }
		public List<CourseDto> Courses { get; set; }
	}
}
