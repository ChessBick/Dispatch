
namespace Dispatch.Application.Dtos
{
    public class ActivityDto
    {
        public string? Id { get; set; }
        public string? WorkOrderId { get; set; }
        public string? Type { get; set; }
        public string? OldStatus { get; set; }
        public string? NewStatus { get; set; }
        public string? Note { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
