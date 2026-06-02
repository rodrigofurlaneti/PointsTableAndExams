using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PointsTableAndExams.Domain.Entities;

namespace PointsTableAndExams.Infrastructure.Data.Configurations;

public sealed class FoodItemConfiguration : IEntityTypeConfiguration<FoodItem>
{
    public void Configure(EntityTypeBuilder<FoodItem> builder)
    {
        builder.ToTable("FoodItem");
        builder.Ignore(e => e.CreatedAt);
        builder.Ignore(e => e.UpdatedAt);
        builder.HasKey(f => f.Id);
        builder.Property(f => f.Name).HasMaxLength(150).IsRequired();
        builder.Property(f => f.ServingSize).HasMaxLength(100);
        builder.Property(f => f.Notes).HasMaxLength(300);

        builder.OwnsOne(f => f.Points, p =>
            p.Property(x => x.Value).HasColumnName("Points").HasConversion(v => (short)v, v => (int)v).IsRequired());

        // Relationship configured in FoodCategoryConfiguration
        builder.HasOne(f => f.Category)
            .WithMany(c => c.Items)
            .HasForeignKey(f => f.FoodCategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
