using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LSC.MentorByKarthik.Domain.Entities;

[Table("MentoringSessionLog")]
public partial class MentoringSessionLog
{
    [Key]
    public int SessionLogId { get; set; }

    public int SlotId { get; set; }

    public int UserId { get; set; }

    public string? Notes { get; set; }

    public string? Feedback { get; set; }

    public int? DurationMinutes { get; set; }

    public DateTime CompletedOn { get; set; }

    [ForeignKey("SlotId")]
    [InverseProperty("MentoringSessionLogs")]
    public virtual MentoringSlot Slot { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("MentoringSessionLogs")]
    public virtual UserProfile User { get; set; } = null!;
}
