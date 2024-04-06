using ApplicationCore.Entities;
using Ardalis.Specification;

namespace ApplicationCore.Specifications
{
	public static class CourseSpecs
	{
		public class All : Specification<Course>
		{
			public All()
			{
				Query.Include(x => x.Teacher)
						.Include(x => x.CoursesStudents)
						.ThenInclude(x => x.Student);
			}
		}
		public class ById : Specification<Course>
		{
			public ById(int Id)
			{
				Query.Where(x => x.Id == Id)
						.Include(x => x.Teacher)
						.Include(x => x.CoursesStudents)
						.ThenInclude(x => x.Student);
			}
		}
		public class ByStudent : Specification<Course>
		{
			public ByStudent(int studentId)
			{
				Query.Where(x => x.CoursesStudents.Any(y => y.StudentId == studentId));
			}
		}
		public class ByTeacher : Specification<Course>
		{
			public ByTeacher(int teacherId)
			{
				Query.Where(x => x.TeacherId == teacherId);
			}
		}
	}
}
