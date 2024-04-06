using ApplicationCore.Entities;
using Ardalis.Specification;

namespace ApplicationCore.Specifications
{
	public static class StudentSpecs
	{
		public class ByCourse : Specification<Student>
		{
			public ByCourse(int courseId)
			{
				Query.Where(x => x.CoursesStudents.Any(y => y.CourseId == courseId));
			}
		}
	}
}
