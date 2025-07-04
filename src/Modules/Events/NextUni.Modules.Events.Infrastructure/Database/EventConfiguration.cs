using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NextUni.Modules.Events.Domain.Events;

namespace NextUni.Modules.Events.Infrastructure.Database;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.HasKey(@event => @event.Id);
        builder.Property(@event => @event.Name).IsRequired();
        builder.Property(@event => @event.Address).IsRequired();
        builder.Property(@event => @event.IsOnline).IsRequired();
        builder.Property(@event => @event.StartDate).IsRequired();
        builder.Property(@event => @event.UniversityId).IsRequired();
        builder.Property(@event => @event.Status).HasConversion<byte>().IsRequired().HasDefaultValue(EventStatus.Pending);
        builder.HasQueryFilter(@event => @event.Status != EventStatus.Pending && 
                                         @event.Status != EventStatus.Rejected);
    }
}