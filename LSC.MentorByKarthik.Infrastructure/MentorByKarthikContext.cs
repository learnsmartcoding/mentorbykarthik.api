using System;
using System.Collections.Generic;
using LSC.MentorByKarthik.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LSC.MentorByKarthik.Infrastructure;

public partial class MentorByKarthikContext : DbContext
{
    public MentorByKarthikContext()
    {
    }

    public MentorByKarthikContext(DbContextOptions<MentorByKarthikContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BannerInfo> BannerInfos { get; set; }

    public virtual DbSet<ContactU> ContactUs { get; set; }

    public virtual DbSet<MentoringSessionLog> MentoringSessionLogs { get; set; }

    public virtual DbSet<MentoringSlot> MentoringSlots { get; set; }

    public virtual DbSet<MentoringSlotRequest> MentoringSlotRequests { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SmartApp> SmartApps { get; set; }

    public virtual DbSet<UserActivityLog> UserActivityLogs { get; set; }

    public virtual DbSet<UserNotification> UserNotifications { get; set; }

    public virtual DbSet<UserProfile> UserProfiles { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BannerInfo>(entity =>
        {
            entity.HasKey(e => e.BannerId).HasName("PK_BannerInfo_BannerId");

            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<ContactU>(entity =>
        {
            entity.HasKey(e => e.ContactUsId).HasName("PK_ContactUs_ContactUsId");
        });

        modelBuilder.Entity<MentoringSessionLog>(entity =>
        {
            entity.HasKey(e => e.SessionLogId).HasName("PK_MentoringSessionLog_SessionLogId");

            entity.HasOne(d => d.Slot).WithMany(p => p.MentoringSessionLogs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MentoringSessionLog_Slot");

            entity.HasOne(d => d.User).WithMany(p => p.MentoringSessionLogs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MentoringSessionLog_User");
        });

        modelBuilder.Entity<MentoringSlot>(entity =>
        {
            entity.HasKey(e => e.SlotId).HasName("PK_MentoringSlot_SlotId");

            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.SlotDurationMinutes).HasDefaultValue(30);
            entity.Property(e => e.Status).HasDefaultValue("Available");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.MentoringSlots)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MentoringSlot_CreatedByUser");
        });

        modelBuilder.Entity<MentoringSlotRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK_MentoringSlotRequest_RequestId");

            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.RequestedByUser).WithMany(p => p.MentoringSlotRequests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MentoringSlotRequest_RequestedByUser");

            entity.HasOne(d => d.Slot).WithMany(p => p.MentoringSlotRequests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MentoringSlotRequest_Slot");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK_Notification_NotificationId");

            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK_Roles_RoleId");
        });

        modelBuilder.Entity<SmartApp>(entity =>
        {
            entity.HasKey(e => e.SmartAppId).HasName("PK_SmartApp_SmartAppId");
        });

        modelBuilder.Entity<UserActivityLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK_UserActivityLog_LogId");

            entity.HasOne(d => d.User).WithMany(p => p.UserActivityLogs).HasConstraintName("FK_UserActivityLog_UserProfile");
        });

        modelBuilder.Entity<UserNotification>(entity =>
        {
            entity.HasKey(e => e.UserNotificationId).HasName("PK_UserNotifications_UserNotificationId");

            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Notification).WithMany(p => p.UserNotifications)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserNotifications_NotificationId");

            entity.HasOne(d => d.User).WithMany(p => p.UserNotifications)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserNotifications_UserId");
        });

        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK_UserProfile_UserId");

            entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DisplayName).HasDefaultValue("Guest");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.UserRoleId).HasName("PK_UserRole_UserRoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserRole_Roles");

            entity.HasOne(d => d.SmartApp).WithMany(p => p.UserRoles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserRole_SmartApp");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserRole_UserProfile");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
