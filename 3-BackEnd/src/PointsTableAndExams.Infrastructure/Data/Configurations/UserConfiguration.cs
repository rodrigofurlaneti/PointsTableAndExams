using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PointsTableAndExams.Domain.Entities;

namespace PointsTableAndExams.Infrastructure.Data.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(u => u.Id);
        builder.Property(u => u.FullName).HasMaxLength(150).IsRequired();
        builder.Property(u => u.Username).HasMaxLength(80).IsRequired();
        builder.Property(u => u.PasswordHash).HasMaxLength(256).IsRequired();
        builder.Property(u => u.Gender).HasConversion<string>().HasMaxLength(10);
        builder.Property(u => u.PhoneNumber).HasMaxLength(20);

        builder.OwnsOne(u => u.Email, e =>
        {
            e.Property(x => x.Value).HasColumnName("Email").HasMaxLength(150).IsRequired();
            e.HasIndex(x => x.Value).IsUnique();
        });

        builder.HasIndex(u => u.Username).IsUnique();
        builder.Ignore(u => u.DomainEvents);
    }
}
