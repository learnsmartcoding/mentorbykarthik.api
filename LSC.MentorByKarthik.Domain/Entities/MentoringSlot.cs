using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LSC.MentorByKarthik.Domain.Entities;

[Table("MentoringSlot")]
public partial class MentoringSlot
{
    [Key]
    public int SlotId { get; set; }

    public int CreatedByUserId { get; set; }

    public DateTime SlotDateTime { get; set; }

    public int SlotDurationMinutes { get; set; }

    [StringLength(50)]
    public string SlotType { get; set; } = null!;

    [StringLength(20)]
    public string Status { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    [ForeignKey("CreatedByUserId")]
    [InverseProperty("MentoringSlots")]
    public virtual UserProfile CreatedByUser { get; set; } = null!;

    [InverseProperty("Slot")]
    public virtual ICollection<MentoringSessionLog> MentoringSessionLogs { get; set; } = new List<MentoringSessionLog>();

    [InverseProperty("Slot")]
    public virtual ICollection<MentoringSlotRequest> MentoringSlotRequests { get; set; } = new List<MentoringSlotRequest>();
}
