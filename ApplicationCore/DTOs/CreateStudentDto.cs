namespace ApplicationCore.DTOs
{
	public class CreateStudentDto
	{
		public string Name { get; set; }
		public string Surname { get; set; }
		public DateTime BirthDate { get; set; }
		public string GroupName { get; set; }
	}
}
