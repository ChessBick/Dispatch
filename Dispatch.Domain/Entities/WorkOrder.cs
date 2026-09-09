using Dispatch.Domain.Enums;

namespace Dispatch.Domain.Entities
{
    public class WorkOrder
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? AssignedTo { get; set; }
        public WorkOrderStatus Status { get; set; } = WorkOrderStatus.Pending;
        public DateTime? ScheduledDate { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public List<WorkOrderActivity> Activities { get; set; } = new();

        public string PublicId => $"wo-{Id}";
    }
}
