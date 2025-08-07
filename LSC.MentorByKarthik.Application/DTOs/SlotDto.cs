using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSC.MentorByKarthik.Application.DTOs
{
    public class CreateMentoringSlotDto
    {
        [Required]
        public DateTime SlotDateTime { get; set; }

        [Required]
        public int SlotDurationMinutes { get; set; }

        [Required]
        [StringLength(50)]
        public string SlotType { get; set; } = string.Empty;
    }

    public class SlotRequestDto
    {
        [Required]
        [StringLength(1000)]
        public string Purpose { get; set; } = string.Empty;
    }

    public class MentoringSlotDto: SlotRequestDto
    {
        public int SlotId { get; set; }
        public DateTime SlotDateTime { get; set; }
        public int SlotDurationMinutes { get; set; }
        public string SlotType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? CreatedBy { get; set; }
    }

    public class MentoringSlotRequestDto
    {
        public int RequestId { get; set; }
        public int SlotId { get; set; }
        public DateTime SlotDateTime { get; set; }
        public string Purpose { get; set; } = string.Empty;
        public bool IsApproved { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public DateTime? CancelledOn { get; set; }
        public string RequestedBy { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    public enum SlotStatus
    {
        Approved = 1,
        Cancelled = 2
    }

    public class MeetingSlotRequest
    {
        public DateTime MeetingSlotTime { get; set; }
        public string UserName { get; set; } = "User";
        public int? UserId { get; set; }
        public string Purpose { get; set; } = string.Empty;
        public string UserEmailId { get; set; } = string.Empty;
        public SlotStatus Status { get; set; } = SlotStatus.Approved; // Default to approved
    }

}
