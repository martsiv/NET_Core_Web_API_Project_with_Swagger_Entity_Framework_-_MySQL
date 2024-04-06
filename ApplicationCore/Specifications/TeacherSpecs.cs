using ApplicationCore.Entities;
using Ardalis.Specification;

namespace ApplicationCore.Specifications
{
	public static class TeacherSpecs
	{
		public class All : Specification<Teacher>
		{
			public All()
			{
				Query.Include(x => x.Courses);
			}
		}
		public class ById : Specification<Teacher>
		{
			public ById(int Id)
			{
				Query.Where(x => x.Id == Id).Include(x => x.Courses);
			}
		}
	}
}
