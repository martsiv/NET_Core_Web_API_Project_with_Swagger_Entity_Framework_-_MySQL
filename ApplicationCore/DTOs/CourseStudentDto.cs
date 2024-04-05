using ApplicationCore.Entities;

namespace ApplicationCore.DTOs
{
	public class CourseStudentDto
	{
		public int Id { get; set; }
		public int CourseId { get; set; }
		public int StudentId { get; set; }
	}
}
