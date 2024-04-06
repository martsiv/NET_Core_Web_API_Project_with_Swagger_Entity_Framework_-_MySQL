using ApplicationCore.Entities;
using Ardalis.Specification;

namespace ApplicationCore.Specifications
{
	public static class CourseStudentSpecs
	{
		public class ByCourseAndStudent : Specification<CourseStudent>
		{
			public ByCourseAndStudent(int courseId, int studentId)
			{
				Query.Where(x => x.CourseId == courseId && x.StudentId == studentId);
			}
		}
	}
}
