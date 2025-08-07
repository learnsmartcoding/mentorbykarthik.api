using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LSC.MentorByKarthik.Domain.Entities;

[Table("UserProfile")]
public partial class UserProfile
{
    [Key]
    public int UserId { get; set; }

    [StringLength(100)]
    public string DisplayName { get; set; } = null!;

    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [StringLength(50)]
    public string LastName { get; set; } = null!;

    [StringLength(100)]
    public string Email { get; set; } = null!;

    [StringLength(128)]
    public string AdObjId { get; set; } = null!;

    [StringLength(100)]
    public string? MyTimeZone { get; set; }

    public DateTime CreatedOn { get; set; }

    [StringLength(500)]
    public string? ProfileImageUrl { get; set; }

    [StringLength(100)]
    public string? PreferedLanguage { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<MentoringSessionLog> MentoringSessionLogs { get; set; } = new List<MentoringSessionLog>();

    [InverseProperty("RequestedByUser")]
    public virtual ICollection<MentoringSlotRequest> MentoringSlotRequests { get; set; } = new List<MentoringSlotRequest>();

    [InverseProperty("CreatedByUser")]
    public virtual ICollection<MentoringSlot> MentoringSlots { get; set; } = new List<MentoringSlot>();

    [InverseProperty("User")]
    public virtual ICollection<UserActivityLog> UserActivityLogs { get; set; } = new List<UserActivityLog>();

    [InverseProperty("User")]
    public virtual ICollection<UserNotification> UserNotifications { get; set; } = new List<UserNotification>();

    [InverseProperty("User")]
    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
