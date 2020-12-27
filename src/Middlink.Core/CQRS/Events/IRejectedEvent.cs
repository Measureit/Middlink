namespace Middlink.Core.CQRS.Events
{
    public interface IRejectedEvent : IDomainEvent
    {
        string Reason { get; }
        string Code { get; }
    }
}
