using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PointsTableAndExams.Domain.Entities;

namespace PointsTableAndExams.Infrastructure.Data.Configurations;

public sealed class DailyLogConfiguration : IEntityTypeConfiguration<DailyLog>
{
    public void Configure(EntityTypeBuilder<DailyLog> builder)
    {
        builder.ToTable("DailyLogs");
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Notes).HasMaxLength(500);
        builder.HasIndex(d => new { d.UserId, d.LogDate }).IsUnique();

        builder.OwnsOne(d => d.TotalPoints, p =>
            p.Property(x => x.Value).HasColumnName("TotalPoints").IsRequired());

        builder.HasMany(d => d.Items)
            .WithOne()
            .HasForeignKey(i => i.DailyLogId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(d => d.DomainEvents);
    }
}
