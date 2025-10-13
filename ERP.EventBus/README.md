# ERP.EventBus - RabbitMQ Event Bus

> 🚀 Shared library สำหรับจัดการ messaging และ event-driven communication ใน ERP System

## 📋 สารบัญ

- [การติดตั้ง](#การติดตั้ง)
- [Configuration](#configuration)
- [การใช้งานพื้นฐาน](#การใช้งานพื้นฐาน)
- [การสร้าง Events](#การสร้าง-events)
- [การสร้าง Message Handlers](#การสร้าง-message-handlers)
- [ตัวอย่างการใช้งาน](#ตัวอย่างการใช้งาน)
- [Best Practices](#best-practices)

## 🔧 การติดตั้ง

### 1. เพิ่ม Project Reference

ใน project ที่ต้องการใช้งาน EventBus ให้เพิ่ม reference:

```xml
<ItemGroup>
  <ProjectReference Include="..\ERP.EventBus\ERP.EventBus.csproj" />
</ItemGroup>
```

### 2. ติดตั้ง RabbitMQ Server

```bash
# Docker
docker run -d --hostname rabbitmq --name rabbitmq -p 15672:15672 -p 5672:5672 rabbitmq:3-management

# หรือติดตั้งแบบ standalone
# Windows: https://www.rabbitmq.com/install-windows.html
# macOS: brew install rabbitmq
# Ubuntu: sudo apt-get install rabbitmq-server
```

## ⚙️ Configuration

### appsettings.json

```json
{
  "RabbitMQ": {
    "HostName": "localhost",
    "Port": 5672,
    "UserName": "guest",
    "Password": "guest",
    "VirtualHost": "/",
    "ExchangeName": "erp_exchange",
    "QueueName": "erp_queue",
    "Durable": true,
    "AutoDelete": false
  }
}
```

### Program.cs (Dependency Injection)

```csharp
using ERP.EventBus;

var builder = WebApplication.CreateBuilder(args);

// เพิ่ม EventBus service
builder.Services.AddEventBus(builder.Configuration);

// เพิ่ม Message Handlers (optional)
builder.Services.AddEventHandler<MyCustomEvent, MyCustomHandler>();

var app = builder.Build();
```

## 🚀 การใช้งานพื้นฐาน

### 1. Inject IEventBus

```csharp
public class UserService
{
    private readonly IEventBus _eventBus;

    public UserService(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }
}
```

### 2. Publishing Events

```csharp
public async Task CreateUserAsync(CreateUserRequest request)
{
    // สร้าง user logic...
    var user = new User { /* ... */ };

    // Publish event
    await _eventBus.PublishAsync(new MyCustomEvent
    {
        // Your event properties here
    });
}
```

### 3. Subscribing to Events

```csharp
public class NotificationService
{
    private readonly IEventBus _eventBus;

    public NotificationService(IEventBus eventBus)
    {
        _eventBus = eventBus;

        // Subscribe to events
        Task.Run(async () =>
        {
            await _eventBus.SubscribeAsync<MyCustomEvent>(HandleMyEvent);
        });
    }

    private async Task HandleMyEvent(MyCustomEvent myEvent)
    {
        // Handle the event
        Console.WriteLine($"Processing event: {myEvent.Id}");
        await Task.CompletedTask;
    }
}
```

## 📝 การสร้าง Events

### สร้าง Custom Event

```csharp
using ERP.EventBus.Events;

namespace YourProject.Events;

public class OrderProcessedEvent : BaseMessage
{
    public override string EventType => nameof(OrderProcessedEvent);

    public Guid OrderId { get; set; }
    public Guid CustomerId { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime ProcessedAt { get; set; }
    public List<OrderItem> Items { get; set; } = new();
}

public class OrderItem
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
```

### การสร้าง Event Class

Events ต้อง implement `IMessage` interface

## 🔨 การสร้าง Message Handlers

### สร้าง Handler Class

```csharp
using ERP.EventBus.Interfaces;

public class OrderProcessedHandler : IMessageHandler<OrderProcessedEvent>
{
    private readonly ILogger<OrderProcessedHandler> _logger;
    private readonly IEmailService _emailService;

    public OrderProcessedHandler(
        ILogger<OrderProcessedHandler> logger,
        IEmailService emailService)
    {
        _logger = logger;
        _emailService = emailService;
    }

    public async Task HandleAsync(OrderProcessedEvent message)
    {
        try
        {
            _logger.LogInformation("Processing order {OrderId}", message.OrderId);

            // ส่ง confirmation email
            await _emailService.SendOrderConfirmationAsync(
                message.CustomerId,
                message.OrderId);

            // Update inventory
            // Send to accounting system
            // etc.

            _logger.LogInformation("Order {OrderId} processed successfully", message.OrderId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to process order {OrderId}", message.OrderId);
            throw;
        }
    }
}
```

### Register Handler

```csharp
// Program.cs
builder.Services.AddEventHandler<OrderProcessedEvent, OrderProcessedHandler>();
```

## 💡 ตัวอย่างการใช้งาน

### E-commerce Order Flow

```csharp
// 1. Order Service - สร้าง order
public class OrderService
{
    private readonly IEventBus _eventBus;

    public async Task ProcessOrderAsync(CreateOrderRequest request)
    {
        var order = new Order { /* ... */ };

        // Save to database
        await _orderRepository.SaveAsync(order);

        // Publish event
        await _eventBus.PublishAsync(new OrderProcessedEvent
        {
            OrderId = order.Id,
            CustomerId = order.CustomerId,
            TotalAmount = order.TotalAmount,
            Status = "Processed",
            ProcessedAt = DateTime.UtcNow,
            Items = order.Items.Select(i => new OrderItem { /* ... */ }).ToList()
        });
    }
}

// 2. Inventory Service - ลด stock
public class InventoryHandler : IMessageHandler<OrderProcessedEvent>
{
    public async Task HandleAsync(OrderProcessedEvent message)
    {
        foreach (var item in message.Items)
        {
            await _inventoryService.ReduceStockAsync(item.ProductId, item.Quantity);
        }
    }
}

// 3. Email Service - ส่ง confirmation
public class EmailHandler : IMessageHandler<OrderProcessedEvent>
{
    public async Task HandleAsync(OrderProcessedEvent message)
    {
        await _emailService.SendOrderConfirmationAsync(message.CustomerId, message.OrderId);
    }
}

// 4. Analytics Service - บันทึก metrics
public class AnalyticsHandler : IMessageHandler<OrderProcessedEvent>
{
    public async Task HandleAsync(OrderProcessedEvent message)
    {
        await _analyticsService.TrackOrderAsync(message.OrderId, message.TotalAmount);
    }
}
```

### User Management Flow

```csharp
// Example: User Registration Event
await _eventBus.PublishAsync(new MyUserEvent
{
    UserId = user.Id,
    Email = user.Email,
    ActionType = "UserRegistered"
});

// Multiple handlers จะทำงานอัตโนมัติ:
// - ส่ง welcome email
// - สร้าง user profile
// - เพิ่มใน CRM system
// - ส่ง notification ให้ admin
```

## 🏆 Best Practices

### 1. Event Naming

```csharp
// ✅ ดี - ใช้ past tense
public class UserCreatedEvent : IMessage
public class OrderProcessedEvent : IMessage
public class PaymentCompletedEvent : IMessage

// ❌ หลีกเลี่ยง - ใช้ present tense
public class CreateUserEvent : IMessage
public class ProcessOrderEvent : IMessage
```

### 2. Event Properties

```csharp
// ✅ ดี - มีข้อมูลครบถ้วน
public class UserCreatedEvent : IMessage
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime Timestamp { get; } = DateTime.UtcNow;
    public string EventType => nameof(UserCreatedEvent);

    public Guid UserId { get; set; }           // Required
    public string Email { get; set; }          // Required
    public string FirstName { get; set; }      // Optional but useful
    public DateTime CreatedAt { get; set; }    // Timestamp
    public Dictionary<string, object> Metadata { get; set; } // Extensible
}
```

### 3. Error Handling

```csharp
public class MyEventHandler : IMessageHandler<MyCustomEvent>
{
    public async Task HandleAsync(MyCustomEvent message)
    {
        try
        {
            // Process message
        }
        catch (TemporaryException ex)
        {
            // Log และ retry
            _logger.LogWarning(ex, "Temporary failure processing event {EventId}", message.Id);
            throw; // Re-queue message
        }
        catch (PermanentException ex)
        {
            // Log และ skip
            _logger.LogError(ex, "Permanent failure processing event {EventId}", message.Id);
            // Don't throw - message will be acknowledged
        }
    }
}
```

### 4. Testing

```csharp
// Unit Test Example
[Test]
public async Task Should_Handle_Event_Correctly()
{
    // Arrange
    var testEvent = new MyCustomEvent
    {
        // Test data properties
    };

    var handler = new MyEventHandler(_mockService.Object);

    // Act
    await handler.HandleAsync(testEvent);

    // Assert
    _mockService.Verify(x => x.ProcessEventAsync(testEvent.Id), Times.Once);
}
```

### 5. Performance Considerations

- ใช้ `async/await` สำหรับ I/O operations
- Handle bulk messages efficiently
- Monitor queue sizes และ processing times
- Use appropriate retry policies

### 6. Security

- ใช้ secure connection (TLS) สำหรับ production
- Implement proper authentication
- Validate message content
- Log security events

## 🚨 Troubleshooting

### Connection Issues

```csharp
// Check RabbitMQ server status
docker exec rabbitmq rabbitmq-diagnostics status

// Check logs
docker logs rabbitmq
```

### Message Not Consumed

1. ตรวจสอบ queue name
2. ตรวจสอบ routing key
3. ตรวจสอบ exchange configuration
4. ตรวจสอบ consumer registration

### Performance Issues

1. Monitor queue depths
2. Scale consumers horizontally
3. Optimize message sizes
4. Use batch processing where appropriate

## 📚 Additional Resources

- [RabbitMQ Documentation](https://www.rabbitmq.com/documentation.html)
- [Event-Driven Architecture Patterns](https://martinfowler.com/articles/201701-event-driven.html)
- [Microservices Messaging Patterns](https://microservices.io/patterns/data/event-sourcing.html)

---

💡 **Tips**: ใช้ RabbitMQ Management UI ที่ `http://localhost:15672` (guest/guest) เพื่อ monitor queues และ exchanges

🤝 **Support**: หากมีปัญหาหรือข้อสงสัย สามารถติดต่อทีม Development ได้