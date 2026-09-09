using Dispatch.Application.Dtos;
using Disptach.Shared.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dispatch.Application.Services
{
    public interface IWorkOrderService
    {
        Task<WorkOrderDto> ChangeWorkOrderStatus(int id, ChangeStatusRequest request);
        Task<WorkOrderDto> CreateWorkOrder(CreateWorkOrderRequest request);
        Task<WorkOrderDto> GetWorkOrder(int id);
        Task<List<ActivityDto>> GetWorkOrderActivities(int id, int take);
        Task<PagedResult<WorkOrderDto>> ListWorkOrders(ListWorkOrdersRequest request);
        Task<WorkOrderDto> UpdateWorkOrder(int id, UpdateWorkOrderRequest request);
    }
}
