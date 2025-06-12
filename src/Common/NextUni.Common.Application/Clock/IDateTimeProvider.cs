namespace NextUni.Common.Application.Clock;

public interface IDateTimeProvider
{
    public DateTime UtcNow { get; }
}