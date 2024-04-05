using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
	internal class CourseStudentConfig : IEntityTypeConfiguration<CourseStudent>
	{
		public void Configure(EntityTypeBuilder<CourseStudent> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("CoursesStudents");
		}
	}
}
