using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations
{
	internal class StudentConfig : IEntityTypeConfiguration<Student>
	{
		public void Configure(EntityTypeBuilder<Student> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("Students");
			builder.HasMany(x => x.CoursesStudents).WithOne(x => x.Student).HasForeignKey(x => x.StudentId).IsRequired(true);
		}
	}
}
