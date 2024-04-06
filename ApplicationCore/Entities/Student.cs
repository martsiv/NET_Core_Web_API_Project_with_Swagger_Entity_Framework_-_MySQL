namespace ApplicationCore.Entities
{
	public class Student
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Surname { get; set; }
		public DateTime BirthDate { get; set; }
        public string GroupName { get; set; }
		public ICollection<CourseStudent> CoursesStudents { get; set; } = new HashSet<CourseStudent>();

	}
}
