using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LSC.MentorByKarthik.Domain.Entities;

[Table("MentoringSlotRequest")]
public partial class MentoringSlotRequest
{
    [Key]
    public int RequestId { get; set; }

    public int SlotId { get; set; }

    public int RequestedByUserId { get; set; }

    [StringLength(1000)]
    public string Purpose { get; set; } = null!;

    public bool IsApproved { get; set; }

    public DateTime? ApprovedOn { get; set; }
    public DateTime? CancelledOn { get; set; }

    public DateTime CreatedOn { get; set; }

    [ForeignKey("RequestedByUserId")]
    [InverseProperty("MentoringSlotRequests")]
    public virtual UserProfile RequestedByUser { get; set; } = null!;

    [ForeignKey("SlotId")]
    [InverseProperty("MentoringSlotRequests")]
    public virtual MentoringSlot Slot { get; set; } = null!;
}
