using Dispatch.Domain.Entities;
using Dispatch.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Dispatch.Infrastructure.Persistence
{
    public static class SeedData
    {
        public static async Task SeedAsync(DispatchDbContext db)
        {
            if (await db.WorkOrders.AnyAsync())
                return;

            var now = DateTime.Now;

            var w1 = new WorkOrder
            {
                Title = "Fix leaking faucet",
                Description = "Kitchen faucet is leaking and needs a new washer.",
                AssignedTo = "John Smith",
                Status = WorkOrderStatus.Pending,
                ScheduledDate = now.AddDays(1),
                CreatedAt = now,
                UpdatedAt = now
            };
            w1.Activities.Add(new WorkOrderActivity
            {
                Type = ActivityType.Created,
                NewStatus = WorkOrderStatus.Pending,
                Note = "Work order created.",
                CreatedAt = now
            });

            var w2 = new WorkOrder
            {
                Title = "Replace HVAC filter",
                Description = "Quarterly maintenance for building HVAC system.",
                AssignedTo = "Jane Doe",
                Status = WorkOrderStatus.InProgress,
                ScheduledDate = now.AddDays(-1),
                StartedAt = now.AddHours(-2),
                CreatedAt = now.AddDays(-2),
                UpdatedAt = now
            };
            w2.Activities.Add(new WorkOrderActivity
            {
                Type = ActivityType.Created,
                NewStatus = WorkOrderStatus.Pending,
                Note = "Work order created.",
                CreatedAt = now.AddDays(-2)
            });
            w2.Activities.Add(new WorkOrderActivity
            {
                Type = ActivityType.StatusChanged,
                OldStatus = WorkOrderStatus.Pending,
                NewStatus = WorkOrderStatus.InProgress,
                Note = "Work started.",
                CreatedAt = now.AddHours(-2)
            });

            var w3 = new WorkOrder
            {
                Title = "Install new light fixtures",
                Description = "Replace old fluorescent lights with LED fixtures.",
                AssignedTo = "Mike Johnson",
                Status = WorkOrderStatus.Done,
                ScheduledDate = now.AddDays(-5),
                StartedAt = now.AddDays(-5),
                CompletedAt = now.AddDays(-4),
                CreatedAt = now.AddDays(-6),
                UpdatedAt = now.AddDays(-4)
            };
            w3.Activities.Add(new WorkOrderActivity
            {
                Type = ActivityType.Created,
                NewStatus = WorkOrderStatus.Pending,
                Note = "Work order created.",
                CreatedAt = now.AddDays(-6)
            });
            w3.Activities.Add(new WorkOrderActivity
            {
                Type = ActivityType.StatusChanged,
                OldStatus = WorkOrderStatus.Pending,
                NewStatus = WorkOrderStatus.InProgress,
                Note = "Work started.",
                CreatedAt = now.AddDays(-5)
            });
            w3.Activities.Add(new WorkOrderActivity
            {
                Type = ActivityType.StatusChanged,
                OldStatus = WorkOrderStatus.InProgress,
                NewStatus = WorkOrderStatus.Done,
                Note = "Work completed.",
                CreatedAt = now.AddDays(-4)
            });

            db.WorkOrders.AddRange(w1, w2, w3);
            await db.SaveChangesAsync();
        }
    }
}
