using AutoMapper;
using Dispatch.Application.Dtos;
using Dispatch.Application.Repository;
using Dispatch.Domain.Entities;
using Dispatch.Domain.Enums;
using Disptach.Shared.Responses;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Text;

namespace Dispatch.Application.Services
{
    public class WorkOrderService(
        IWorkOrderRepository repository,
        IValidator<CreateWorkOrderRequest> createValidator,
        IValidator<UpdateWorkOrderRequest> updateValidator,
        IValidator<ChangeStatusRequest> statusValidator, IMapper mapper) : IWorkOrderService
    {

        private static readonly Dictionary<WorkOrderStatus, WorkOrderStatus[]> AllowedTransitions = new()
        {
            [WorkOrderStatus.Pending] = new[] { WorkOrderStatus.InProgress, WorkOrderStatus.Canceled },
            [WorkOrderStatus.InProgress] = new[] { WorkOrderStatus.Done, WorkOrderStatus.Canceled },
            [WorkOrderStatus.Done] = Array.Empty<WorkOrderStatus>(),
            [WorkOrderStatus.Canceled] = Array.Empty<WorkOrderStatus>()
        };
        public async Task<WorkOrderDto> CreateWorkOrder(CreateWorkOrderRequest request)
        {
            await createValidator.ValidateAndThrowAsync(request);
            var now = DateTime.Now;
            var workOrder = new WorkOrder
            {
                Title = request.Title.Trim(),
                Description = request.Description?.Trim(),
                AssignedTo = request.AssignedTo?.Trim(),
                ScheduledDate = request.ScheduledDate,
                Status = WorkOrderStatus.Pending,
                CreatedAt = now,
                UpdatedAt = now,
            };
            workOrder.Activities.Add(Create(workOrder, ActivityType.Created, null, WorkOrderStatus.Pending, "Work order created"));

            repository.Add(workOrder);
            await repository.SaveChanges();

            return mapper.Map<WorkOrderDto>(workOrder);
        }

        public async Task<WorkOrderDto> GetWorkOrder(int id)
        {
            var workOrder = await repository.GetById(id)
                ?? throw new Exception($"WorkOrder with id {id} not found.");
            return mapper.Map<WorkOrderDto>(workOrder);
        }

        public async Task<PagedResult<WorkOrderDto>> ListWorkOrders(ListWorkOrdersRequest request)
        {

            WorkOrderStatus? status = string.IsNullOrWhiteSpace(request.Status)
                ? null
                : Enum.Parse<WorkOrderStatus>(request.Status, ignoreCase: true);

            var (items, totalCount) = await repository.ListWorkOrders(status, request.Page, request.PageSize);
            var dtos = items.Select(item => mapper.Map<WorkOrderDto>(item)).ToList();

            return PagedResult<WorkOrderDto>.Create(dtos, request.Page, request.PageSize, totalCount);
        }

        public async Task<WorkOrderDto> UpdateWorkOrder(int id, UpdateWorkOrderRequest request)
        {
            await updateValidator.ValidateAndThrowAsync(request);

            var workOrder = await repository.GetById(id)
                ?? throw new Exception($"WorkOrder with id {id} not found.");
            workOrder = UpdateDetails(workOrder, request.Title, request.Description, request.AssignedTo, request.ScheduledDate);


            await repository.SaveChanges();

            return mapper.Map<WorkOrderDto>(workOrder);
        }

        public async Task<WorkOrderDto> ChangeWorkOrderStatus(int id, ChangeStatusRequest request)
        {
            await statusValidator.ValidateAndThrowAsync(request);

            var workOrder = await repository.GetById(id)
                ?? throw new Exception($"WorkOrder with id {id} not found.");

            var newStatus = Enum.Parse<WorkOrderStatus>(request.Status, ignoreCase: true);
           workOrder = ChangeStatus(workOrder, newStatus, request.Note);

            await repository.SaveChanges();
            return mapper.Map<WorkOrderDto>(workOrder);
        }

        public async Task<List<ActivityDto>> GetWorkOrderActivities(int id, int take)
        {
            if (!await repository.Exists(id))
                throw new Exception($"WorkOrder with id {id} not found.");

            take = take is <= 0 or > 200 ? 20 : take;

            var activities = await repository.GetByIdWithActivities(id);
            return activities?.Activities.Take(take).Select(item => mapper.Map<ActivityDto>(item)).ToList() ?? new List<ActivityDto>();
        }

        #region - Private Helpers -
        private WorkOrder ChangeStatus(WorkOrder workOrder, WorkOrderStatus newStatus, string? note)
        {
            if (workOrder.Status == newStatus)
                throw new Exception($"Work order is already in status '{workOrder.Status}'.");

            if (!AllowedTransitions[workOrder.Status].Contains(newStatus))
            {
                throw new Exception(
                    $"Cannot transition from '{workOrder.Status}' to '{newStatus}'. Allowed: {string.Join(", ", AllowedTransitions[workOrder.Status])}.");
            }

            var oldStatus = workOrder.Status;
            var now = DateTime.UtcNow;

            workOrder.Status = newStatus;
            workOrder.UpdatedAt = now;

            if (newStatus == WorkOrderStatus.InProgress && workOrder.StartedAt == null)
                workOrder.StartedAt = now;
            if (newStatus == WorkOrderStatus.Done)
                workOrder.CompletedAt = now;

            workOrder.Activities.Add(Create(workOrder, ActivityType.StatusChanged, oldStatus, newStatus, note));
            return workOrder;
        }
        private WorkOrder UpdateDetails(WorkOrder workOrder, string title, string? description, string? assignedTo, DateTime? scheduledDate)
        {
            workOrder.Title = title.Trim();
            workOrder.Description = description?.Trim();
            workOrder.AssignedTo = assignedTo?.Trim();
            workOrder.ScheduledDate = scheduledDate;
            workOrder.UpdatedAt = DateTime.UtcNow;

            workOrder.Activities.Add(Create(workOrder, ActivityType.Updated, null, null, "Work order details updated"));
            return workOrder;
        }
        private WorkOrderActivity Create(WorkOrder workOrder,ActivityType type,WorkOrderStatus? oldStatus,WorkOrderStatus? newStatus,string? note)
        {
            return new WorkOrderActivity
            {
                WorkOrderId = workOrder.Id,
                WorkOrder = workOrder,
                Type = type,
                OldStatus = oldStatus,
                NewStatus = newStatus,
                Note = note,
                CreatedAt = DateTime.UtcNow
            };
        }
        #endregion
    }
}