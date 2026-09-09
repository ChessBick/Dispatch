using Dispatch.Domain.Entities;
using Dispatch.Domain.Enums;


namespace Dispatch.Application.Repository
{
    public interface IWorkOrderRepository
    {
        void Add(WorkOrder workOrder);
        Task<bool> Exists(int id);
        Task<List<WorkOrderActivity>> GetActivities(int workOrderId, int take);
        Task<WorkOrder?> GetById(int id);
        Task<WorkOrder?> GetByIdWithActivities(int id);
        Task<(List<WorkOrder> Items, int TotalCount)> ListWorkOrders(WorkOrderStatus? status, int page, int pageSize);
        Task SaveChanges();
    }
}
