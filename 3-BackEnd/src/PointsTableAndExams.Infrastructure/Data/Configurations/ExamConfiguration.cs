using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PointsTableAndExams.Domain.Entities;

namespace PointsTableAndExams.Infrastructure.Data.Configurations;

public sealed class ExamConfiguration : IEntityTypeConfiguration<Exam>
{
    public void Configure(EntityTypeBuilder<Exam> builder)
    {
        builder.ToTable("Exam");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Name).HasMaxLength(150).IsRequired();
        builder.Property(e => e.Abbreviation).HasMaxLength(50);
        builder.Property(e => e.Description).HasMaxLength(300);
        builder.Property(e => e.IsActive).IsRequired();

        builder.HasOne(e => e.Category)
            .WithMany(c => c.Exams)
            .HasForeignKey(e => e.ExamCategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
