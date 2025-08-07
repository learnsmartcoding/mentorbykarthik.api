use [master]
go

IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'MentorByKarthik')
BEGIN
    -- Create the database
    CREATE DATABASE MentorByKarthik;
END
ELSE
BEGIN
   DROP DATABASE MentorByKarthik;
END

Go
use MentorByKarthik
go

-- User Profile Table
CREATE TABLE UserProfile (
    UserId INT IDENTITY(1,1),
	DisplayName NVARCHAR(100) NOT NULL CONSTRAINT DF_UserProfile_DisplayName DEFAULT 'Guest',
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
	AdObjId NVARCHAR(128) NOT NULL,    
	MyTimeZone  NVARCHAR(100) NULL,
    ProfileImageUrl NVARCHAR(500) NULL,
    PreferedLanguage NVARCHAR(100) NULL,
    CreatedOn DATETIME2 NOT NULL CONSTRAINT DF_UserProfile_CreatedOn DEFAULT GETUTCDATE(),
    -- Add other user-related fields as needed
    CONSTRAINT PK_UserProfile_UserId PRIMARY KEY (UserId)
);

--Roles
CREATE TABLE Roles (
    RoleId INT IDENTITY(1,1),    
    RoleName NVARCHAR(50) NOT NULL, --Admin, ReadOnly, Support, Customer etc
    CONSTRAINT PK_Roles_RoleId PRIMARY KEY (RoleId)    
);

-- SmartApp Table. We can use this databse as centralized and add more tables for future apps. that is why this table is added
CREATE TABLE SmartApp (
    SmartAppId INT IDENTITY(1,1),    
    AppName NVARCHAR(50) NOT NULL,    
    CONSTRAINT PK_SmartApp_SmartAppId PRIMARY KEY (SmartAppId)
);

-- UserRole Table
CREATE TABLE UserRole (
    UserRoleId INT IDENTITY(1,1),
    RoleId INT NOT NULL,
    UserId INT NOT NULL,    
	SmartAppId INT NOT NULL,
    CONSTRAINT PK_UserRole_UserRoleId PRIMARY KEY (UserRoleId),
    CONSTRAINT FK_UserRole_UserProfile FOREIGN KEY (UserId) REFERENCES UserProfile(UserId),
    CONSTRAINT FK_UserRole_Roles FOREIGN KEY (RoleId) REFERENCES Roles(RoleId),
	CONSTRAINT FK_UserRole_SmartApp FOREIGN KEY (SmartAppId) REFERENCES SmartApp(SmartAppId)
);

CREATE TABLE Notification (
    NotificationId INT IDENTITY(1,1),
    Subject NVARCHAR(200) NOT NULL, -- Subject of the notification/email
    Content NVARCHAR(MAX) NOT NULL, -- Email body or notification content
    CreatedOn DATETIME2 NOT NULL CONSTRAINT DF_Notification_CreatedOn DEFAULT GETDATE(),
    ScheduledSendTime DATETIME2 NOT NULL, -- When the notification is scheduled to be sent
    IsActive BIT NOT NULL DEFAULT 1, -- If active, it will trigger user notifications
    CONSTRAINT PK_Notification_NotificationId PRIMARY KEY (NotificationId)
);

CREATE TABLE UserNotifications (
    UserNotificationId INT IDENTITY(1,1),
    NotificationId INT NOT NULL,
    UserId INT NOT NULL,
    EmailSubject NVARCHAR(200) NOT NULL, -- Personalized email subject
    EmailContent NVARCHAR(MAX) NOT NULL, -- Personalized email body
    NotificationSent BIT NOT NULL DEFAULT 0, -- Flag to indicate if the email was sent
    SentOn DATETIME2 NULL, -- Time when the email was sent
    CreatedOn DATETIME2 NOT NULL CONSTRAINT DF_UserNotifications_CreatedOn DEFAULT GETDATE(),
    CONSTRAINT PK_UserNotifications_UserNotificationId PRIMARY KEY (UserNotificationId),
    CONSTRAINT FK_UserNotifications_NotificationId FOREIGN KEY (NotificationId) REFERENCES Notification(NotificationId),
    CONSTRAINT FK_UserNotifications_UserId FOREIGN KEY (UserId) REFERENCES UserProfile(UserId)
);

CREATE TABLE BannerInfo (
    BannerId INT IDENTITY(1,1),
    Title NVARCHAR(100) NOT NULL, -- Banner title or heading
    Content NVARCHAR(MAX) NOT NULL, -- Banner content or description
    ImageUrl NVARCHAR(500) NULL, -- Optional URL for banner image
    IsActive BIT NOT NULL DEFAULT 1, -- Only active banners are displayed in the app
    DisplayFrom DATETIME2 NOT NULL, -- Start date for displaying the banner
    DisplayTo DATETIME2 NOT NULL, -- End date for displaying the banner
    CreatedOn DATETIME2 NOT NULL CONSTRAINT DF_BannerInfo_CreatedOn DEFAULT GETDATE(),
    CONSTRAINT PK_BannerInfo_BannerId PRIMARY KEY (BannerId)
);


-- UserActivityLog Table
CREATE TABLE UserActivityLog (
    LogId INT IDENTITY(1,1),
    UserId INT,
    ActivityType NVARCHAR(50) NOT NULL,
    ActivityDescription NVARCHAR(MAX),
    LogDate DATETIME NOT NULL,
    -- Add other log-related fields as needed
    CONSTRAINT PK_UserActivityLog_LogId PRIMARY KEY (LogId),
    CONSTRAINT FK_UserActivityLog_UserProfile FOREIGN KEY (UserId) REFERENCES UserProfile(UserId)
);

--Contact Us table
CREATE TABLE ContactUs (
    ContactUsId INT IDENTITY(1,1),
    UserName NVARCHAR(100) NOT NULL,
	UserEmail NVARCHAR(100) NOT NULL,
	MessageDetail NVARCHAR(2000) NOT NULL,    
    CONSTRAINT PK_ContactUs_ContactUsId PRIMARY KEY (ContactUsId)    
);

CREATE TABLE MentoringSlot (
    SlotId INT IDENTITY(1,1),
    CreatedByUserId INT NOT NULL, -- Admin UserId
    SlotDateTime DATETIME2 NOT NULL, -- Stored in UTC
    SlotDurationMinutes INT NOT NULL CONSTRAINT DF_MentoringSlot_Duration DEFAULT 30,
    SlotType NVARCHAR(50) NOT NULL, -- VideoCall, WhatsAppCall, GroupCall
    Status NVARCHAR(20) NOT NULL CONSTRAINT DF_MentoringSlot_Status DEFAULT 'Available', -- Available, Requested, Approved
    CreatedOn DATETIME2 NOT NULL CONSTRAINT DF_MentoringSlot_CreatedOn DEFAULT GETUTCDATE(),
    CONSTRAINT PK_MentoringSlot_SlotId PRIMARY KEY (SlotId),
    CONSTRAINT FK_MentoringSlot_CreatedByUser FOREIGN KEY (CreatedByUserId) REFERENCES UserProfile(UserId)
);

CREATE TABLE MentoringSlotRequest (
    RequestId INT IDENTITY(1,1),
    SlotId INT NOT NULL,
    RequestedByUserId INT NOT NULL,
    Purpose NVARCHAR(1000) NOT NULL,
    IsApproved BIT NOT NULL CONSTRAINT DF_MentoringSlotRequest_IsApproved DEFAULT 0,
    ApprovedOn DATETIME2 NULL,
    CancelledOn DATETIME2 NULL,    
    CreatedOn DATETIME2 NOT NULL CONSTRAINT DF_MentoringSlotRequest_CreatedOn DEFAULT GETUTCDATE(),
    CONSTRAINT PK_MentoringSlotRequest_RequestId PRIMARY KEY (RequestId),
    CONSTRAINT FK_MentoringSlotRequest_Slot FOREIGN KEY (SlotId) REFERENCES MentoringSlot(SlotId),
    CONSTRAINT FK_MentoringSlotRequest_RequestedByUser FOREIGN KEY (RequestedByUserId) REFERENCES UserProfile(UserId)
);

CREATE TABLE MentoringSessionLog (
    SessionLogId INT IDENTITY(1,1),
    SlotId INT NOT NULL,
    UserId INT NOT NULL,
    Notes NVARCHAR(MAX) NULL, -- Any follow-up notes
    Feedback NVARCHAR(MAX) NULL,
    DurationMinutes INT NULL,
    CompletedOn DATETIME2 NOT NULL,
    CONSTRAINT PK_MentoringSessionLog_SessionLogId PRIMARY KEY (SessionLogId),
    CONSTRAINT FK_MentoringSessionLog_Slot FOREIGN KEY (SlotId) REFERENCES MentoringSlot(SlotId),
    CONSTRAINT FK_MentoringSessionLog_User FOREIGN KEY (UserId) REFERENCES UserProfile(UserId)
);




-- Insert default roles
USE [MentorByKarthik]
GO

INSERT INTO [dbo].[Roles]
           ([RoleName])
     VALUES
           ('Admin')
		   ,('Customer')
GO

INSERT INTO [dbo].[SmartApp]
           ([AppName])
     VALUES
           ('Mentoring') -- we can keep more meaningful name
GO



