using Domain.Entities.Roles;
using Domain.Entities.UserRegistrations;
using Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public sealed class UserRegistrationConfiguration : IEntityTypeConfiguration<UserRegistration>
{
    public void Configure(EntityTypeBuilder<UserRegistration> builder)
    {
        builder.HasKey(userRegistration => userRegistration.Id);
        
        builder.Property(userRegistration => userRegistration.Id)
            .HasConversion(
                userRegistrationId => userRegistrationId.Value,
                value => new UserRegistrationId(value));
        
        builder.OwnsOne(userRegistration => userRegistration.FullName, name =>
        {
            name.Property(name => name.Firstname)
                .HasColumnName("FirstName");
            name.Property(name => name.Lastname)
                .HasColumnName("LastName");
        });
        
        builder.Property(userRegistration => userRegistration.Email)
            .HasConversion(
                email => email.Value,
                value => Email.Create(value));

        builder.Property(userRegistration => userRegistration.Role)
            .HasConversion(
                role => role.Value,
                value => Role.FromString(value).Value);

        builder.HasIndex(userRegistration => userRegistration.Email).IsUnique();
        
        builder.OwnsOne(userRegistration => userRegistration.PasswordHash, passwordHash =>
        {
            passwordHash.Property(passwordHash => passwordHash.Value).HasColumnName("PasswordHash");
        });

        builder.Property(userRegistration => userRegistration.RegisteredAtUtc)
            .IsRequired();
        
        builder.OwnsOne(userRegistration => userRegistration.Status, status =>
        {
            status.Property(status => status.Value).HasColumnName("Status");
        });
        
        builder.OwnsOne(userRegistration => userRegistration.ConfirmationCode, confirmationCode =>
        {
            confirmationCode.Property(confirmationCode => 
                confirmationCode.CodeHash).HasColumnName("ConfirmationCodeHash");
        });

        builder.Property(userRegistration => userRegistration.ConfirmedAtUtc).IsRequired(false);
    }
}