using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PointsTableAndExams.Domain.Entities;

namespace PointsTableAndExams.Infrastructure.Data.Configurations;

public sealed class FoodCategoryConfiguration : IEntityTypeConfiguration<FoodCategory>
{
    public void Configure(EntityTypeBuilder<FoodCategory> builder)
    {
        builder.ToTable("FoodCategory");
        builder.Ignore(e => e.CreatedAt);
        builder.Ignore(e => e.UpdatedAt);
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name).HasMaxLength(100).IsRequired();
        builder.Property(c => c.Description).HasMaxLength(300);
        builder.Property(c => c.DefaultQuotaPoints).HasConversion(v => v.HasValue ? (short?)v.Value : null, v => v.HasValue ? (int?)v.Value : null);
        builder.Property(c => c.ServingUnit).HasMaxLength(100);
        builder.Property(c => c.SortOrder).IsRequired();
        builder.Property(c => c.IsActive).IsRequired().HasDefaultValue(true);

        // Relationship defined on FoodItemConfiguration side
    }
}
