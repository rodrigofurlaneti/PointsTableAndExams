using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PointsTableAndExams.Domain.Entities;
using PointsTableAndExams.Domain.ValueObjects;

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

        // Gender: enum → single char stored as string
        builder.Property(u => u.Gender)
            .HasConversion<string>()
            .HasMaxLength(10);

        // Email: Value Object → owned entity mapped to single column
        builder.OwnsOne(u => u.Email, e =>
        {
            e.Property(x => x.Value)
                .HasColumnName("Email")
                .HasMaxLength(150)
                .IsRequired();
            e.HasIndex(x => x.Value).IsUnique();
        });

        // PhoneNumber: Value Object → value converter (string ↔ PhoneNumber)
        builder.Property(u => u.PhoneNumber)
            .HasMaxLength(20)
            .HasConversion(
                pn => pn == null ? null : pn.Value,
                value => value == null ? null : PhoneNumber.Create(value));

        builder.HasIndex(u => u.Username).IsUnique();
        builder.Ignore(u => u.DomainEvents);
    }
}
