namespace ApplicationCore.Entities
{
	public class Teacher
	{
        public int Id { get; set; }
		public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public int WorkExperienceFullYears { get; set; }
        public ICollection<Course> Courses { get; set; } = new HashSet<Course>();
        public ICollection<LessonEvent> LessonEvents { get; set; } = new HashSet<LessonEvent>();
    }
}
