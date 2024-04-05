namespace ApplicationCore.Entities
{
	public class Course
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public int TeacherId { get; set; }
        public Teacher? Teacher { get; set; }
		public ICollection<CourseStudent> CoursesStudents { get; set; } = new HashSet<CourseStudent>();
	}
}
