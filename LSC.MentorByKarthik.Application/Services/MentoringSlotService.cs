using AutoMapper;
using LSC.MentorByKarthik.Application.DTOs;
using LSC.MentorByKarthik.Application.Interfaces;
using LSC.MentorByKarthik.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSC.MentorByKarthik.Application.Services
{
    public class MentoringSlotService : IMentoringSlotService
    {
        private readonly IMentoringSlotRepository _repository;
        private readonly IMapper _mapper;

        public MentoringSlotService(IMentoringSlotRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task CreateSlotAsync(CreateMentoringSlotDto dto, int createdByUserId)
        {
            var slot = _mapper.Map<MentoringSlot>(dto);
            slot.CreatedByUserId = createdByUserId;
            slot.CreatedOn = DateTime.UtcNow;
            slot.Status = "Available";

            await _repository.AddSlotAsync(slot);
        }

        public async Task<IEnumerable<MentoringSlotDto>> GetAvailableSlotsAsync()
        {
            var slots = await _repository.GetAvailableSlotsAsync();
            return _mapper.Map<IEnumerable<MentoringSlotDto>>(slots);
        }

        public async Task<IEnumerable<MentoringSlotDto>> GetAllSlotsAsync()
        {
            var slots = await _repository.GetAllSlotsAsync();
            return _mapper.Map<IEnumerable<MentoringSlotDto>>(slots);
        }

        public async Task<MentoringSlotDto?> GetSlotByIdAsync(int slotId)
        {
            var slot = await _repository.GetSlotByIdAsync(slotId);
            return slot == null ? null : _mapper.Map<MentoringSlotDto>(slot);
        }

        public async Task RequestSlotAsync(int slotId, int userId, SlotRequestDto dto)
        {
            var request = new MentoringSlotRequest
            {
                SlotId = slotId,
                RequestedByUserId = userId,
                Purpose = dto.Purpose,
                CreatedOn = DateTime.UtcNow,
                IsApproved = false
            };
            await _repository.AddRequestAsync(request);
        }

        public async Task<IEnumerable<MentoringSlotDto>> GetRequestedSlotsByUserAsync(int userId)
        {
            var requests = await _repository.GetRequestsByUserIdAsync(userId);
            return requests.Select(r => _mapper.Map<MentoringSlotDto>(r.Slot));
        }

        public async Task<IEnumerable<MentoringSlotRequestDto>> GetRequestsBySlotIdAsync(int slotId)
        {
            var requests = await _repository.GetRequestsBySlotIdAsync(slotId);
            return _mapper.Map<IEnumerable<MentoringSlotRequestDto>>(requests);
        }
        public async Task<IEnumerable<MentoringSlotRequestDto>> GetRequestSlotsAsync()
        {
            var requests = await _repository.GetRequestSlotsAsync();
            return _mapper.Map<IEnumerable<MentoringSlotRequestDto>>(requests);
        }
        public async Task ApproveSlotRequestAsync(int requestId)
        {
            var request = await _repository.GetRequestByIdAsync(requestId)
                ?? throw new KeyNotFoundException("Request not found");

            request.IsApproved = true;
            request.ApprovedOn = DateTime.UtcNow;
            request.Slot.Status = "Booked";

            await _repository.SaveChangesAsync();
        }

        public async Task CancelSlotRequestAsync(int requestId)
        {
            var request = await _repository.GetRequestByIdAsync(requestId)
                ?? throw new KeyNotFoundException("Request not found");

            request.IsApproved = false;
            request.Slot.Status = "Cancelled";
            request.CancelledOn = DateTime.UtcNow;
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteSlotAsync(int slotId)
        {
            await _repository.DeleteSlotAsync(slotId);
            
        }

        public async Task CancelAssociatedSlotRequest(int slotId)
        {
             await _repository.DisApproveAssociatedSlotRequest(slotId);
        }
        public async Task<bool> IsSlotBookedByUserAsync(int slotId, int userId)
        {
            return await _repository.IsSlotBookedByUserAsync(slotId, userId);
        }
        public Task<MeetingSlotRequest?> GetMeetingSlotDetailsByRequestIdAsync(int requestId)
        {
            return  _repository.GetMeetingSlotDetailsByRequestIdAsync(requestId);
        }
    }

}
