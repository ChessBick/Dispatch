using AutoMapper;
using Dispatch.Application.Dtos;
using Dispatch.Domain.Entities;

namespace Dispatch.Api.Mapping
{
    public class WorkOrderMappingProfile : Profile
    {
        public WorkOrderMappingProfile()
        {
            CreateMap<WorkOrder, WorkOrderDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString())).ReverseMap();

            CreateMap<WorkOrderActivity, ActivityDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PublicId))
                .ForMember(dest => dest.WorkOrderId, opt => opt.MapFrom(src => src.WorkOrderId.ToString()))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
                .ForMember(dest => dest.OldStatus, opt => opt.MapFrom(src => src.OldStatus != null ? src.OldStatus.ToString() : null))
                .ForMember(dest => dest.NewStatus, opt => opt.MapFrom(src => src.NewStatus != null ? src.NewStatus.ToString() : null))
                .ReverseMap();
        }
    }
}
