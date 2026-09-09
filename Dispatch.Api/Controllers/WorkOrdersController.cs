using Dispatch.Application.Dtos;
using Dispatch.Application.Services;
using Disptach.Shared.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Dispatch.Api.Controllers
{
    [ApiController]
    [Route("api/workorders")]
    public class WorkOrdersController(IWorkOrderService service) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<WorkOrderDto>>>> List(
            [FromQuery] ListWorkOrdersRequest request)
        {
            var result = await service.ListWorkOrders(request);
            return Ok(new ApiResponse<PagedResult<WorkOrderDto>>
            {
                Data = result,
                Success = true,
                Message = null
            }); 
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<WorkOrderDto>>> Get(int id)
        {
            var dto = await service.GetWorkOrder(id);
            return Ok(new ApiResponse<WorkOrderDto>
            {
                Data = dto,
                Success = true,
                Message = null
            });
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<WorkOrderDto>>> Create(
            [FromBody] CreateWorkOrderRequest request)
        {
            var created = await service.CreateWorkOrder(request);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, new ApiResponse<WorkOrderDto>
            {
                Data = created,
                Success = true,
                Message = null
            });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<WorkOrderDto>>> Update(
            int id, [FromBody] UpdateWorkOrderRequest request)
        {
            var dto = await service.UpdateWorkOrder(id, request);
            return Ok(new ApiResponse<WorkOrderDto>
            {
                Data = dto,
                Success = true,
                Message = null
            });
        }

        [HttpPatch("{id}/status")]
        public async Task<ActionResult<ApiResponse<WorkOrderDto>>> ChangeStatus(
            int id, [FromBody] ChangeStatusRequest request)
        {
            var dto = await service.ChangeWorkOrderStatus(id, request);
            return Ok(new ApiResponse<WorkOrderDto>
            {
                Data = dto,
                Success = true,
                Message = null
            });
        }

        [HttpGet("{id}/activities")]
        public async Task<ActionResult<ApiResponse<IReadOnlyList<ActivityDto>>>> GetActivities(
            int id, [FromQuery] int take = 20)
        {
            var activities = await service.GetWorkOrderActivities(id, take);
            return Ok(new ApiResponse<IReadOnlyList<ActivityDto>>
            {
                Data = activities,
                Success = true,
                Message = null
            });
        }
    }
 }
