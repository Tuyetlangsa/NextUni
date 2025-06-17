using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NextUni.Modules.Events.Domain.Events;

namespace NextUni.Modules.Events.Infrastructure.Database;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(user => user.Id);
        builder.Property(user => user.Email).IsRequired();
        builder.Property(user => user.PhoneNumber).IsRequired();
        builder.Property(user => user.FirstName ).IsRequired();
        builder.Property(user => user.LastName).IsRequired();
    }
}