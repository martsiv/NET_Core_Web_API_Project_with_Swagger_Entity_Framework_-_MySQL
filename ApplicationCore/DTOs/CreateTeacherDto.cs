namespace ApplicationCore.DTOs
{
	public class CreateTeacherDto
	{
		public string Name { get; set; }
		public string Surname { get; set; }
		public DateTime BirthDate { get; set; }
		public int WorkExperienceFullYears { get; set; }
	}
}
