namespace ERP.EventBus.Interfaces;

public interface IMessage
{
    Guid Id { get; }
    DateTime Timestamp { get; }
    string EventType { get; }
}