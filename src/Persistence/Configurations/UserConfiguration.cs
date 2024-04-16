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

        builder.Property(user => user.PasswordHash)
            .HasConversion(
                hash => hash.Value,
                value => PasswordHash.Create(value));

        builder.OwnsMany(user => user.Roles, role =>
        {
            role.WithOwner().HasForeignKey("UserId");
            role.ToTable("UserRoles");
            role.Property<UserId>("UserId");
            role.Property<string>("Value").HasColumnName("RoleCode");
            role.HasKey("UserId", "Value");
        });

        //builder.OwnsMany<Role>("_roles", b =>
        //{
        //    b.WithOwner().HasForeignKey("UserId");
        //    b.ToTable("UserRoles");
        //    b.Property<UserId>("UserId");
        //    b.Property<string>("Value").HasColumnName("RoleCode");
        //    b.HasKey("UserId", "Value");
        //});

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

        builder.HasMany(user => user.Deliveries)
            .WithOne(delivery => delivery.User)
            .HasForeignKey(delivery => delivery.UserId);
    }
}