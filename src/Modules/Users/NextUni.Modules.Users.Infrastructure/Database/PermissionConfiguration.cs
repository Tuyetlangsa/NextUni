using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NextUni.Modules.Users.Domain.Users;

namespace NextUni.Modules.Users.Infrastructure.Database;

internal sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("permissions");

        builder.HasKey(p => p.Code);

        builder.Property(p => p.Code).HasMaxLength(100);

        // builder.HasData(
        //     Permission.GetUser,
        //     Permission.ModifyUser,
        //     Permission.GetEvents,
        //     Permission.SearchEvents,
        //     Permission.ModifyEvents,
        //     Permission.GetTicketTypes,
        //     Permission.ModifyTicketTypes,
        //     Permission.GetCategories,
        //     Permission.ModifyCategories,
        //     Permission.GetCart,
        //     Permission.AddToCart,
        //     Permission.RemoveFromCart,
        //     Permission.GetOrders,
        //     Permission.CreateOrder,
        //     Permission.GetTickets,
        //     Permission.CheckInTicket,
        //     Permission.GetEventStatistics);
        builder.HasData(Permission.CreateUniversity);

        builder
            .HasMany<Role>()
            .WithMany()
            .UsingEntity(joinBuilder =>
            {
                joinBuilder.ToTable("role_permissions");

                joinBuilder.HasData(
                    CreateRolePermission(Role.Student, Permission.CreateUniversity));
            });
    }

    private static object CreateRolePermission(Role role, Permission permission)
    {
        return new
        {
            RoleName = role.Name,
            PermissionCode = permission.Code
        };
    }
}
