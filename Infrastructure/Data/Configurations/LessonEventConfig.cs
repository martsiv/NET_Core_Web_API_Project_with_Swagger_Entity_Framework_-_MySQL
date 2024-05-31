using ApplicationCore.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Configurations
{
    internal class LessonEventConfig : IEntityTypeConfiguration<LessonEvent>
    {
        public void Configure(EntityTypeBuilder<LessonEvent> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("LessonEvents");
            builder.HasOne(x => x.Teacher).WithMany(x => x.LessonEvents).HasForeignKey(x => x.TeacherId).IsRequired(true);
        }
    }
}
