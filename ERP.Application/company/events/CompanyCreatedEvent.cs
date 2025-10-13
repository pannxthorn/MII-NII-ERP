using ERP.ApplicationDTO.branch;
using ERP.EventBus.Interfaces;

namespace ERP.Application.company.events;

public class CompanyCreatedEvent : IMessage
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime Timestamp { get; } = DateTime.UtcNow;
    public string EventType => nameof(CompanyCreatedEvent);

    public int CompanyId { get; set; }
    public string CompanyCode { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public List<CreateBranchDTO> BranchDTOs { get; set; } = new();
}