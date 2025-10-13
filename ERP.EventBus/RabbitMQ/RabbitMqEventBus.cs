using ERP.EventBus.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ERP.EventBus.RabbitMQ;

public class RabbitMqEventBus : IEventBus, IDisposable
{
    private readonly RabbitMqConnection _connection;
    private readonly RabbitMqOptions _options;
    private readonly ILogger<RabbitMqEventBus> _logger;
    private readonly Dictionary<string, List<Func<object, Task>>> _handlers = new();
    private IModel? _channel;
    private bool _disposed;

    public RabbitMqEventBus(
        RabbitMqConnection connection,
        IOptions<RabbitMqOptions> options,
        ILogger<RabbitMqEventBus> logger)
    {
        _connection = connection;
        _options = options.Value;
        _logger = logger;
        InitializeChannel();
    }

    public async Task PublishAsync<T>(T message) where T : class, IMessage
    {
        try
        {
            var eventName = typeof(T).Name;
            var jsonMessage = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(jsonMessage);

            _channel?.BasicPublish(
                exchange: _options.ExchangeName,
                routingKey: eventName,
                basicProperties: null,
                body: body);

            _logger.LogInformation("Published event {EventName} with ID {MessageId}", eventName, message.Id);
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish event {EventName}", typeof(T).Name);
            throw;
        }
    }

    public async Task SubscribeAsync<T>(Func<T, Task> handler) where T : class, IMessage
    {
        var eventName = typeof(T).Name;

        if (!_handlers.ContainsKey(eventName))
            _handlers[eventName] = new List<Func<object, Task>>();

        _handlers[eventName].Add(async (obj) => await handler((T)obj));

        SetupConsumer(eventName);

        _logger.LogInformation("Subscribed to event {EventName}", eventName);
        await Task.CompletedTask;
    }

    public async Task UnsubscribeAsync<T>() where T : class, IMessage
    {
        var eventName = typeof(T).Name;
        _handlers.Remove(eventName);

        _logger.LogInformation("Unsubscribed from event {EventName}", eventName);
        await Task.CompletedTask;
    }

    private void InitializeChannel()
    {
        var connection = _connection.GetConnection();
        _channel = connection.CreateModel();

        _channel.ExchangeDeclare(
            exchange: _options.ExchangeName,
            type: ExchangeType.Direct,
            durable: _options.Durable,
            autoDelete: _options.AutoDelete);

        _logger.LogInformation("RabbitMQ channel initialized");
    }

    private void SetupConsumer(string eventName)
    {
        var queueName = $"{_options.QueueName}_{eventName}";

        _channel?.QueueDeclare(
            queue: queueName,
            durable: _options.Durable,
            exclusive: false,
            autoDelete: _options.AutoDelete);

        _channel?.QueueBind(
            queue: queueName,
            exchange: _options.ExchangeName,
            routingKey: eventName);

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                if (_handlers.ContainsKey(eventName))
                {
                    // Find event type from loaded assemblies
                    var eventType = AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(a => a.GetTypes())
                        .FirstOrDefault(t => t.Name == eventName && typeof(IMessage).IsAssignableFrom(t));
                    if (eventType != null)
                    {
                        var eventObj = JsonConvert.DeserializeObject(message, eventType);
                        if (eventObj != null)
                        {
                            foreach (var handler in _handlers[eventName])
                            {
                                await handler(eventObj);
                            }
                        }
                    }
                }

                _channel?.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message for event {EventName}", eventName);
                _channel?.BasicNack(ea.DeliveryTag, false, false);
            }
        };

        _channel?.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
    }

    public void Dispose()
    {
        if (_disposed)
            return;

        _channel?.Close();
        _channel?.Dispose();
        _connection?.Dispose();
        _disposed = true;

        _logger.LogInformation("RabbitMQ EventBus disposed");
    }
}