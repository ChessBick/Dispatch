using Dispatch.Domain.Enums;

namespace Dispatch.Domain.Entities
{
    public class WorkOrderActivity
    {
        public int Id { get; set; }

        public int WorkOrderId { get; set; }
        public WorkOrder? WorkOrder { get; set; }

        public ActivityType Type { get; set; }

        public WorkOrderStatus? OldStatus { get; set; }
        public WorkOrderStatus? NewStatus { get; set; }

        public string? Note { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string PublicId => $"act-{Id}";
    }
}
