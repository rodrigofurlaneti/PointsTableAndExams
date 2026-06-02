using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PointsTableAndExams.Domain.Entities;

namespace PointsTableAndExams.Infrastructure.Data.Configurations;

public sealed class ExamRequestConfiguration : IEntityTypeConfiguration<ExamRequest>
{
    public void Configure(EntityTypeBuilder<ExamRequest> builder)
    {
        builder.ToTable("ExamRequest");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.DoctorName).HasMaxLength(150);
        builder.Property(r => r.Notes).HasMaxLength(500);

        builder.HasMany(r => r.Items)
            .WithOne()
            .HasForeignKey(i => i.ExamRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(r => r.DomainEvents);
    }
}
