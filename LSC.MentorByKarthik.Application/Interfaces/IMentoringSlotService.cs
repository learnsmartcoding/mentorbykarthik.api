using LSC.MentorByKarthik.Application.DTOs;
using LSC.MentorByKarthik.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSC.MentorByKarthik.Application.Interfaces
{
    public interface IMentoringSlotService
    {
        Task CreateSlotAsync(CreateMentoringSlotDto dto, int createdByUserId);
        Task<IEnumerable<MentoringSlotDto>> GetAvailableSlotsAsync();
        Task<IEnumerable<MentoringSlotDto>> GetAllSlotsAsync();
        Task<MentoringSlotDto?> GetSlotByIdAsync(int slotId);
        Task RequestSlotAsync(int slotId, int userId, SlotRequestDto dto);
        Task<IEnumerable<MentoringSlotDto>> GetRequestedSlotsByUserAsync(int userId);
        Task<IEnumerable<MentoringSlotRequestDto>> GetRequestsBySlotIdAsync(int slotId);
        Task ApproveSlotRequestAsync(int requestId);
        Task CancelSlotRequestAsync(int requestId);
        Task DeleteSlotAsync(int slotId);
        Task CancelAssociatedSlotRequest(int slotId);
        Task<IEnumerable<MentoringSlotRequestDto>> GetRequestSlotsAsync();
        Task<bool> IsSlotBookedByUserAsync(int slotId, int userId);
        Task<MeetingSlotRequest?> GetMeetingSlotDetailsByRequestIdAsync(int requestId);
    }


}
