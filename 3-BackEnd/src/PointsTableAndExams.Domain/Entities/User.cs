using PointsTableAndExams.Domain.Common;
using PointsTableAndExams.Domain.DomainEvents;
using PointsTableAndExams.Domain.Enums;
using PointsTableAndExams.Domain.Exceptions;
using PointsTableAndExams.Domain.ValueObjects;

namespace PointsTableAndExams.Domain.Entities;

public sealed class User : AggregateRoot
{
    public string FullName { get; private set; } = string.Empty;
    public Email Email { get; private set; } = null!;
    public PhoneNumber? PhoneNumber { get; private set; }
    public DateOnly? BirthDate { get; private set; }
    public Gender Gender { get; private set; }
    public string Username { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }

    private User() { }

    public static User Create(
        string fullName,
        string email,
        string? phoneNumber,
        DateOnly? birthDate,
        Gender gender,
        string username,
        string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new DomainException("Full name cannot be empty.");

        if (string.IsNullOrWhiteSpace(username))
            throw new DomainException("Username cannot be empty.");

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainException("Password hash cannot be empty.");

        var user = new User
        {
            FullName = fullName.Trim(),
            Email = Email.Create(email),
            PhoneNumber = phoneNumber is not null ? ValueObjects.PhoneNumber.Create(phoneNumber) : null,
            BirthDate = birthDate,
            Gender = gender,
            Username = username.Trim().ToLowerInvariant(),
            PasswordHash = passwordHash,
            IsActive = true
        };

        user.SetCreatedAt(DateTime.UtcNow);
        user.RaiseDomainEvent(UserCreatedEvent.Create(user.Id, user.Email.Value));

        return user;
    }

    public void UpdateProfile(string fullName, string? phoneNumber, DateOnly? birthDate)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new DomainException("Full name cannot be empty.");

        FullName = fullName.Trim();
        PhoneNumber = phoneNumber is not null ? ValueObjects.PhoneNumber.Create(phoneNumber) : null;
        BirthDate = birthDate;
        SetUpdatedAt(DateTime.UtcNow);
    }

    public void ChangePassword(string newPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(newPasswordHash))
            throw new DomainException("Password hash cannot be empty.");
        PasswordHash = newPasswordHash;
        SetUpdatedAt(DateTime.UtcNow);
    }

    public void Deactivate() { IsActive = false; SetUpdatedAt(DateTime.UtcNow); }
    public void Activate()   { IsActive = true;  SetUpdatedAt(DateTime.UtcNow); }
}
