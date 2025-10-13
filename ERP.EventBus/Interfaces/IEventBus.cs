namespace ERP.EventBus.Interfaces;

public interface IEventBus
{
    Task PublishAsync<T>(T message) where T : class, IMessage;
    Task SubscribeAsync<T>(Func<T, Task> handler) where T : class, IMessage;
    Task UnsubscribeAsync<T>() where T : class, IMessage;
}