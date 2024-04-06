namespace ApplicationCore.DTOs
{
	public class TeacherViewDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Surname { get; set; }
		public DateTime BirthDate { get; set; }
		public int WorkExperienceFullYears { get; set; }
        public List<CourseDto> Courses { get; set; }
    }
}
