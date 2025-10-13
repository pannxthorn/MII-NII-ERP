namespace ERP.EventBus.RabbitMQ;

public class RabbitMqOptions
{
    public const string SectionName = "RabbitMQ";

    public string HostName { get; set; } = "localhost";
    public int Port { get; set; } = 5672;
    public string UserName { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public string VirtualHost { get; set; } = "/";
    public string ExchangeName { get; set; } = "erp_exchange";
    public string QueueName { get; set; } = "erp_queue";
    public bool Durable { get; set; } = true;
    public bool AutoDelete { get; set; } = false;
}