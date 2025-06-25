using NextUni.Common.Domain;

namespace NextUni.Modules.Events.Domain.Events
{
    public class EventErrors
    {
        public static Error NotFound(Guid eventId) =>
        Error.NotFound("Event.NotFound", $"The event with the identifier {eventId} not found");

        public static Error IncorrectStatus(Guid eventId, EventStatus status) =>
            Error.Conflict("Event.IncorrectStatus", $"The event status " +
                           $"with the identifier {eventId} is {status}");
    }
}