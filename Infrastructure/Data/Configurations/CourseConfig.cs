using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
	internal class CourseConfig : IEntityTypeConfiguration<Course>
	{
		public void Configure(EntityTypeBuilder<Course> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("Courses");
			builder.HasOne(x => x.Teacher).WithMany(x => x.Courses).HasForeignKey(x => x.TeacherId).IsRequired(true);
			builder.HasMany(x => x.CoursesStudents).WithOne(x => x.Course).HasForeignKey(x => x.CourseId).IsRequired(true);
		}
	}
}
