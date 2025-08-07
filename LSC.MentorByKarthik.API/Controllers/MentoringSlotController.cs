using LSC.MentorByKarthik.Application.DTOs;
using LSC.MentorByKarthik.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LSC.MentorByKarthik.API.Controllers
{
    // MentoringSlotController.cs
    [Route("api/[controller]")]
    [ApiController]
    public class MentoringSlotController : ControllerBase
    {
        private readonly IMentoringSlotService _service;
        private readonly IUserClaims _userClaims;

        public MentoringSlotController(IMentoringSlotService service, IUserClaims userClaims)
        {
            _service = service;
            _userClaims = userClaims;
        }

        // 1. Create a mentoring slot (Admin)
        [HttpPost()]
        public async Task<IActionResult> CreateSlot([FromBody] CreateMentoringSlotDto dto)
        {
            var userId = await _userClaims.GetUserUserIdAsync();
            await _service.CreateSlotAsync(dto, userId);
            return Ok();
        }

        // 2. Get all available slots (User)
        [HttpGet("available")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAvailableSlots()
        {
            var slots = await _service.GetAvailableSlotsAsync();
            return Ok(slots);
        }

        // 3. Get all slots (Admin)
        [HttpGet("admin")]
        public async Task<IActionResult> GetAllSlots()
        {
            var slots = await _service.GetAllSlotsAsync();
            return Ok(slots);
        }

        // 4. Request a slot (User)
        [HttpPost("{slotId}/request")]
        public async Task<IActionResult> RequestSlot(int slotId, [FromBody] SlotRequestDto dto)
        {
            var userId = await _userClaims.GetUserUserIdAsync();
            var isSlotBooked = await _service.IsSlotBookedByUserAsync(slotId, userId);
            if (isSlotBooked)
            {
                return BadRequest(new { Msg = "Slot has been booked already" });
            }
            await _service.RequestSlotAsync(slotId, userId, dto);
            return Ok();
        }

        // 5. Approve a slot request (Admin)
        [HttpPut("{requestId}/approve")]
        public async Task<IActionResult> ApproveSlotRequest(int requestId)
        {
            await _service.ApproveSlotRequestAsync(requestId);          
            return Ok();
        }

        // 6. Cancel a slot request (User or Admin)
        [HttpPut("{requestId}/cancel")]
        public async Task<IActionResult> CancelSlotRequest(int requestId)
        {
            await _service.CancelSlotRequestAsync(requestId);                    
            return Ok();
        }

        // 7. Delete a slot (Admin)
        [HttpDelete("{slotId}")]
        public async Task<IActionResult> DeleteSlot(int slotId)
        {
            await _service.DeleteSlotAsync(slotId);
            //Now pull all associated request for the slot and mark them as cancelled.
            await _service.CancelAssociatedSlotRequest(slotId);
            return NoContent();
        }

        // 8. Get slots requested/booked by current user
        [HttpGet("mine")]
        public async Task<IActionResult> GetMyRequestedSlots()
        {
            var userId = await _userClaims.GetUserUserIdAsync();
            var slots = await _service.GetRequestedSlotsByUserAsync(userId);
            return Ok(slots);
        }

        // 9. Get a single slot by ID
        [HttpGet("{slotId}")]
        public async Task<IActionResult> GetSlotById(int slotId)
        {
            var slot = await _service.GetSlotByIdAsync(slotId);
            return slot == null ? NotFound() : Ok(slot);
        }

        // 10. Get all requests for a specific slot (Admin)
        [HttpGet("{slotId}/requests")]
        public async Task<IActionResult> GetRequestsBySlotId(int slotId)
        {
            
            var requests = await _service.GetRequestsBySlotIdAsync(slotId);
            return Ok(requests);
        }

        // 11. Get all requests for a future slots (Admin)
        [HttpGet("slot-requests")]
        public async Task<IActionResult> GetSlotRequests()
        {
            var requests = await _service.GetRequestSlotsAsync();
            return Ok(requests);
        }
    }

}
