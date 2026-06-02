using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PointsTableAndExams.Domain.Entities;

namespace PointsTableAndExams.Infrastructure.Data.Configurations;

public sealed class DailyLogItemConfiguration : IEntityTypeConfiguration<DailyLogItem>
{
    public void Configure(EntityTypeBuilder<DailyLogItem> builder)
    {
        builder.ToTable("DailyLogItem");
        builder.Ignore(e => e.CreatedAt);
        builder.Ignore(e => e.UpdatedAt);
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Quantity).HasColumnType("decimal(5,2)").IsRequired();
        builder.Property(i => i.PointsComputed).IsRequired();
        builder.Property(i => i.Notes).HasMaxLength(300);
    }
}
