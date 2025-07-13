using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NextUni.Modules.Events.Domain.Events;

namespace NextUni.Modules.Events.Infrastructure.Database;

public class EventRegistrationConfiguration : IEntityTypeConfiguration<EventRegistration>
{
    public void Configure(EntityTypeBuilder<EventRegistration> builder)
    {
        builder.HasKey(e => e.Id);
        builder.ToTable("event_registrations");
        builder.Property(e => e.Id);
        builder.Property(e => e.EventId)
            .IsRequired();
        builder.Property(e => e.UserId).IsRequired();
        // builder.Property(e => e.Status).IsRequired().HasDefaultValue(true);
    }
}