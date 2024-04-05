using ApplicationCore.Entities;
using Ardalis.Specification;

namespace ApplicationCore.Specifications
{
	public static class StudentSpecs
	{
		public class ByGroup : Specification<Student>
		{
            public ByGroup(string groupName)
            {
				Query.Where(x => x.GroupName == groupName);
			}
		}
		public class OlderThanDate : Specification<Student>
		{
			public OlderThanDate(DateTime date)
			{
				Query.Where(x => x.BirthDate < date);
			}
		}
	}
}
