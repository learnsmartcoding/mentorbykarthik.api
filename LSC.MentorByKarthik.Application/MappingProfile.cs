using AutoMapper;
using LSC.MentorByKarthik.Application.DTOs;
using LSC.MentorByKarthik.Domain.Entities;

namespace LSC.MentorByKarthik.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateMentoringSlotDto, MentoringSlot>()
     .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom(_ => DateTime.UtcNow))
     .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => "Available"));

            CreateMap<MentoringSlot, MentoringSlotDto>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedByUser.DisplayName)
                )
                ;

            CreateMap<MentoringSlotRequest, MentoringSlotRequestDto>()
                .ForMember(dest => dest.RequestedBy, opt => opt.MapFrom(src => src.RequestedByUser.DisplayName))
                .ForMember(dest => dest.SlotDateTime, opt => opt.MapFrom(src => src.Slot.SlotDateTime))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Slot.Status));

        }
    }
}
