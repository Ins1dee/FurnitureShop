using Domain.Entities.RefreshSessions;
using Domain.Entities.Roles;
using Domain.Entities.Users;
using Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(user => user.Id);
        
        builder.Property(user => user.Id)
            .HasConversion(
                userId => userId.Value,
                value => new UserId(value));
        
        builder.OwnsOne(user => user.FullName, name =>
        {
            name.Property(name => name.Firstname)
                .HasColumnName("FirstName");
            
            name.Property(name => name.Lastname)
                .HasColumnName("LastName");
        });
        
        builder.Property(user => user.Email)
            .HasConversion(
                email => email.Value,
                value => Email.Create(value));

        builder.HasIndex(user => user.Email).IsUnique();
        
        builder.OwnsOne(userRegistration => userRegistration.PasswordHash, passwordHash =>
        {
            passwordHash.Property(passwordHash => passwordHash.Value).HasColumnName("PasswordHash");
        });
        
        builder.OwnsMany(user => user.Roles, role =>
        {
            role
                .WithOwner()
                .HasForeignKey(role => role.UserId);
            
            role.ToTable("UserRoles");
            
            role.Property(role => role.Id)
                .HasConversion(
                    roleId => roleId.Value,
                    value => new RoleId(value));
            
            role.Property(role => role.UserId);
            
            role.HasKey(role => new { role.UserId, role.Id });
        });

        builder.OwnsMany(user => user.RefreshSessions, refreshSession =>
        {
            refreshSession
                .WithOwner()
                .HasForeignKey(refreshSession => refreshSession.UserId);

            refreshSession.ToTable("RefreshSessions");
            
            refreshSession.Property(refreshSession => refreshSession.Id)
                .HasConversion(
                    refreshSessionId => refreshSessionId.Value,
                    value => new RefreshSessionId(value));
            
            refreshSession.HasKey(refreshSession => refreshSession.Id);
        });

        builder.HasMany(user => user.Orders)
            .WithOne(order => order.User)
            .HasForeignKey(order => order.UserId);
    }
}