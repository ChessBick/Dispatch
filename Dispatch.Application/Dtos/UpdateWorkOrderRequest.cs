using System;
using System.Collections.Generic;
using System.Text;

namespace Dispatch.Application.Dtos
{
    public class UpdateWorkOrderRequest
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? AssignedTo { get; set; }
        public DateTime? ScheduledDate { get; set; }
    }
}
