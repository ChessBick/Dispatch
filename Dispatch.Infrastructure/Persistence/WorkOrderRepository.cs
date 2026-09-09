using Dispatch.Application.Repository;
using Dispatch.Domain.Entities;
using Dispatch.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Dispatch.Infrastructure.Persistence
{
    public class WorkOrderRepository(DispatchDbContext db) : IWorkOrderRepository
    {
        public Task<WorkOrder?> GetById(int id) =>
            db.WorkOrders.FirstOrDefaultAsync(w => w.Id == id);

        public Task<WorkOrder?> GetByIdWithActivities(int id) =>
            db.WorkOrders.Include(w => w.Activities).FirstOrDefaultAsync(w => w.Id == id);

        public async Task<(List<WorkOrder> Items, int TotalCount)> ListWorkOrders(
            WorkOrderStatus? status, int page, int pageSize)
        {
            var query = db.WorkOrders.AsNoTracking().AsQueryable();
            if (status is not null)
                query = query.Where(w => w.Status == status);

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderByDescending(w => w.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<List<WorkOrderActivity>> GetActivities(int workOrderId, int take) =>
            await db.Activities.AsNoTracking()
                .Where(a => a.WorkOrderId == workOrderId)
                .OrderByDescending(a => a.CreatedAt)
                .Take(take)
                .ToListAsync();

        public Task<bool> Exists(int id) =>
            db.WorkOrders.AnyAsync(w => w.Id == id);

        public void Add(WorkOrder workOrder) => db.WorkOrders.Add(workOrder);

        public Task SaveChanges() => db.SaveChangesAsync();
    }
}
