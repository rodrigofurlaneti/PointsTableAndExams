using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PointsTableAndExams.Domain.Entities;

namespace PointsTableAndExams.Infrastructure.Data.Configurations;

public sealed class ExamRequestItemConfiguration : IEntityTypeConfiguration<ExamRequestItem>
{
    public void Configure(EntityTypeBuilder<ExamRequestItem> builder)
    {
        builder.ToTable("ExamRequestItem");
        builder.Ignore(e => e.CreatedAt);
        builder.Ignore(e => e.UpdatedAt);
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Result).HasMaxLength(500);
        builder.Property(i => i.Laboratory).HasMaxLength(150);
        builder.Property(i => i.Notes).HasMaxLength(300);

        builder.HasOne(i => i.Exam)
            .WithMany()
            .HasForeignKey(i => i.ExamId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
