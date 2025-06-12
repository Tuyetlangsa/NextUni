using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NextUni.Modules.Users.Domain.Users;

namespace NextUni.Modules.Users.Infrastructure.Database;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(user => user.Id);
        builder.Property(user => user.Email).IsRequired();
        builder.Property(user => user.PhoneNumber).IsRequired();
        builder.Property(user => user.FirstName ).IsRequired();
        builder.Property(user => user.LastName).IsRequired();
        builder.HasIndex(user => user.IdentityId).IsUnique();
    }
}