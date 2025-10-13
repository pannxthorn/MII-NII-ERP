namespace ERP.EventBus.Interfaces;

public interface IMessageHandler<in T> where T : class, IMessage
{
    Task HandleAsync(T message);
}