using ApplicationCore.Entities;
using Ardalis.Specification;

namespace ApplicationCore.Specifications
{
	public static class CourseSpecs
	{
		public class ByStudent : Specification<Course>
		{
			public ByStudent(int studentId)
			{
				Query.Where(x => x.CoursesStudents.Any(y => y.StudentId == studentId));
			}
		}
	}
}
