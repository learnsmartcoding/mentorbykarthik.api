using LSC.MentorByKarthik.Application.DTOs;
using LSC.MentorByKarthik.Domain.Entities;

namespace LSC.MentorByKarthik.Application.Interfaces
{
    public interface IMentoringSlotRepository
    {
        Task AddSlotAsync(MentoringSlot slot);
        Task<MentoringSlot?> GetSlotByIdAsync(int slotId);
        Task<IEnumerable<MentoringSlot>> GetAllSlotsAsync();
        Task<IEnumerable<MentoringSlot>> GetAvailableSlotsAsync();
        Task AddRequestAsync(MentoringSlotRequest request);
        Task<MentoringSlotRequest?> GetRequestByIdAsync(int requestId);
        Task<IEnumerable<MentoringSlotRequest>> GetRequestsBySlotIdAsync(int slotId);
        Task<IEnumerable<MentoringSlotRequest>> GetRequestsByUserIdAsync(int userId);
        Task DeleteSlotAsync(int slotId);
        Task DisApproveAssociatedSlotRequest(int slotId);
        Task<List<MentoringSlotRequest>> GetRequestSlotsAsync();
        Task SaveChangesAsync();
        Task<bool> IsSlotBookedByUserAsync(int slotId, int userId);

        Task<MeetingSlotRequest?> GetMeetingSlotDetailsByRequestIdAsync(int requestId);
    }


}
