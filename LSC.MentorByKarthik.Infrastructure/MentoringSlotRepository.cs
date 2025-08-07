using LSC.MentorByKarthik.Application.DTOs;
using LSC.MentorByKarthik.Application.Interfaces;
using LSC.MentorByKarthik.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LSC.MentorByKarthik.Infrastructure
{
    public class MentoringSlotRepository : IMentoringSlotRepository
    {
        private readonly MentorByKarthikContext _context;

        public MentoringSlotRepository(MentorByKarthikContext context)
        {
            _context = context;
        }

        public async Task AddSlotAsync(MentoringSlot slot)
        {
            _context.MentoringSlots.Add(slot);
            await _context.SaveChangesAsync();
        }

        public async Task<MentoringSlot?> GetSlotByIdAsync(int slotId)
        {
            return await _context.MentoringSlots
                .Include(s => s.CreatedByUser)
                .Include(s => s.MentoringSlotRequests)
                .FirstOrDefaultAsync(s => s.SlotId == slotId);
        }

        public async Task<IEnumerable<MentoringSlot>> GetAllSlotsAsync()
        {
            return await _context.MentoringSlots
                .Include(s => s.CreatedByUser)
                .Where(w => w.SlotDateTime >= DateTime.Now)
                //(w => w.Status == "Available")
                .ToListAsync();
        }

        public async Task<IEnumerable<MentoringSlot>> GetAvailableSlotsAsync()
        {
            return await _context.MentoringSlots
                .Where(s => s.Status == "Available")
                .Include(s => s.CreatedByUser)
                .ToListAsync();
        }

        public async Task AddRequestAsync(MentoringSlotRequest request)
        {
            _context.MentoringSlotRequests.Add(request);
            await _context.SaveChangesAsync();
        }

        public async Task<MentoringSlotRequest?> GetRequestByIdAsync(int requestId)
        {
            return await _context.MentoringSlotRequests
                .Include(r => r.Slot)
                .Include(r => r.RequestedByUser)
                .FirstOrDefaultAsync(r => r.RequestId == requestId);
        }

        public async Task<IEnumerable<MentoringSlotRequest>> GetRequestsBySlotIdAsync(int slotId)
        {
            return await _context.MentoringSlotRequests
                .Where(r => r.SlotId == slotId)
                .Include(r => r.Slot)
                .Include(r => r.RequestedByUser)
                .ToListAsync();
        }

        public async Task<IEnumerable<MentoringSlotRequest>> GetRequestsByUserIdAsync(int userId)
        {
            return await _context.MentoringSlotRequests
                .Where(r => r.RequestedByUserId == userId)
                .Include(r => r.Slot)
                .ToListAsync();
        }

        public async Task DeleteSlotAsync(int slotId)
        {
            var slot = await _context.MentoringSlots.FindAsync(slotId);
            if (slot != null)
            {
                //_context.MentoringSlots.Remove(slot);
                slot.Status = "Cancelled"; //Soft delete
                _context.MentoringSlots.Update(slot);
                await _context.SaveChangesAsync();
            }
        }
        public async Task DisApproveAssociatedSlotRequest(int slotId)
        {
            var slots = await _context.MentoringSlotRequests.Where(w => w.SlotId == slotId).ToListAsync();
            foreach (var item in slots)
            {
                item.IsApproved = false;
            }
            _context.MentoringSlotRequests.UpdateRange(slots);
            await _context.SaveChangesAsync();
        }

        public async Task<List<MentoringSlotRequest>> GetRequestSlotsAsync()
        {
            return await _context.MentoringSlotRequests
                .Include(r => r.Slot)
                    .Include(s => s.RequestedByUser)
                //.Where(r => r.Slot.SlotDateTime >= DateTime.UtcNow)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsSlotBookedByUserAsync(int slotId, int userId)
        {
            var data = await _context.MentoringSlotRequests.FirstOrDefaultAsync(f => f.SlotId == slotId && f.RequestedByUserId == userId);
            return data != null;
        }

        public async Task<MeetingSlotRequest?> GetMeetingSlotDetailsByRequestIdAsync(int requestId)
        {
            return await (from slotRequest in _context.MentoringSlotRequests
                          join slot in _context.MentoringSlots on slotRequest.SlotId equals slot.SlotId
                          where slotRequest.RequestId == requestId
                          select new MeetingSlotRequest
                          {
                              MeetingSlotTime = slot.SlotDateTime,
                              Purpose = slotRequest.Purpose,
                              UserName = slotRequest.RequestedByUser.DisplayName,
                              UserEmailId = slotRequest.RequestedByUser.Email,
                              UserId = slotRequest.RequestedByUserId
                          }).FirstOrDefaultAsync();
        }

    }

}
