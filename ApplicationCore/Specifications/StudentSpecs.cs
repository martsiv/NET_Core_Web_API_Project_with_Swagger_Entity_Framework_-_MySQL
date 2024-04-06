using ApplicationCore.Entities;
using Ardalis.Specification;

namespace ApplicationCore.Specifications
{
	public static class StudentSpecs
	{
		public class All : Specification<Student>
		{
			public All()
			{
				Query.Include(x => x.CoursesStudents)
						.ThenInclude(x => x.Course);
			}
		}
		public class ById : Specification<Student>
		{
			public ById(int Id)
			{
				Query.Where(x => x.Id == Id)
						.Include(x => x.CoursesStudents)
						.ThenInclude(x => x.Course);
			}
		}
		public class ByCourse : Specification<Student>
		{
			public ByCourse(int courseId)
			{
				Query.Where(x => x.CoursesStudents.Any(y => y.CourseId == courseId))
						.Include(x => x.CoursesStudents)
						.ThenInclude(x => x.Course);
			}
		}
	}
}
