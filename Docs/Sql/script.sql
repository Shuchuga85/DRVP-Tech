/****** Object:  Database [DanceSchoolDb]    Script Date: 3/14/2026 6:19:30 PM ******/
CREATE DATABASE [DanceSchoolDb]  (EDITION = 'GeneralPurpose', SERVICE_OBJECTIVE = 'GP_S_Gen5_1', MAXSIZE = 32 GB) WITH CATALOG_COLLATION = SQL_Latin1_General_CP1_CI_AS, LEDGER = OFF;
GO
ALTER DATABASE [DanceSchoolDb] SET COMPATIBILITY_LEVEL = 170
GO
ALTER DATABASE [DanceSchoolDb] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [DanceSchoolDb] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [DanceSchoolDb] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [DanceSchoolDb] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [DanceSchoolDb] SET ARITHABORT OFF 
GO
ALTER DATABASE [DanceSchoolDb] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [DanceSchoolDb] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [DanceSchoolDb] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [DanceSchoolDb] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [DanceSchoolDb] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [DanceSchoolDb] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [DanceSchoolDb] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [DanceSchoolDb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [DanceSchoolDb] SET ALLOW_SNAPSHOT_ISOLATION ON 
GO
ALTER DATABASE [DanceSchoolDb] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [DanceSchoolDb] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [DanceSchoolDb] SET  MULTI_USER 
GO
ALTER DATABASE [DanceSchoolDb] SET ENCRYPTION ON
GO
ALTER DATABASE [DanceSchoolDb] SET QUERY_STORE = ON
GO
ALTER DATABASE [DanceSchoolDb] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 100, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
/*** The scripts of database scoped configurations in Azure should be executed inside the target database connection. ***/
GO
-- ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 8;
GO
/****** Object:  Table [dbo].[App_Setting]    Script Date: 3/14/2026 6:19:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[App_Setting](
	[setting_id] [int] IDENTITY(1,1) NOT NULL,
	[setting_key] [nvarchar](64) NULL,
	[setting_value] [nvarchar](128) NULL,
	[updated_at] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[setting_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Blocked_Period]    Script Date: 3/14/2026 6:19:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Blocked_Period](
	[blocked_id] [int] IDENTITY(1,1) NOT NULL,
	[start_datetime] [datetime2](7) NOT NULL,
	[end_datetime] [datetime2](7) NOT NULL,
	[scope] [tinyint] NOT NULL,
	[id_coach] [int] NULL,
	[id_studio] [int] NULL,
	[reason] [nvarchar](128) NULL,
PRIMARY KEY CLUSTERED 
(
	[blocked_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Coach]    Script Date: 3/14/2026 6:19:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Coach](
	[coach_id] [int] NOT NULL,
	[biography] [nvarchar](256) NULL,
	[photo_url] [nvarchar](256) NULL,
PRIMARY KEY CLUSTERED 
(
	[coach_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Coach_Availability]    Script Date: 3/14/2026 6:19:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Coach_Availability](
	[coachav_id] [int] IDENTITY(1,1) NOT NULL,
	[id_coach] [int] NOT NULL,
	[weekday] [tinyint] NOT NULL,
	[start_time] [time](7) NOT NULL,
	[end_time] [time](7) NOT NULL,
	[valid_from] [date] NULL,
	[valid_until] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[coachav_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Coach_Class]    Script Date: 3/14/2026 6:19:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Coach_Class](
	[class_id] [int] IDENTITY(1,1) NOT NULL,
	[id_modality] [int] NOT NULL,
	[id_studio] [int] NOT NULL,
	[id_coach] [int] NOT NULL,
	[created_by] [int] NOT NULL,
	[start_datetime] [datetime2](7) NOT NULL,
	[end_datetime] [datetime2](7) NOT NULL,
	[status] [tinyint] NOT NULL,
	[max_participants] [int] NOT NULL,
	[created_at] [date] NOT NULL,
	[coach_validated_at] [datetime2](7) NULL,
	[staff_validated_at] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[class_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Coach_Modality]    Script Date: 3/14/2026 6:19:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Coach_Modality](
	[id_coach] [int] NOT NULL,
	[id_modality] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id_coach] ASC,
	[id_modality] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Event]    Script Date: 3/14/2026 6:19:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Event](
	[event_id] [int] IDENTITY(1,1) NOT NULL,
	[title] [nvarchar](64) NOT NULL,
	[description] [nvarchar](256) NULL,
	[start_datetime] [datetime2](7) NULL,
	[end_datetime] [datetime2](7) NULL,
	[image_url] [nvarchar](256) NULL,
	[is_active] [bit] NOT NULL,
	[created_by] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[event_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Item]    Script Date: 3/14/2026 6:19:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Item](
	[item_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](128) NOT NULL,
	[description] [nvarchar](256) NULL,
	[from_school] [bit] NOT NULL,
	[id_owner] [int] NULL,
	[id_category] [int] NULL,
	[id_contact] [int] NULL,
	[created_at] [date] NULL,
	[is_active] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[item_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Item_Category]    Script Date: 3/14/2026 6:19:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Item_Category](
	[category_id] [int] IDENTITY(1,1) NOT NULL,
	[catg_name] [nvarchar](128) NULL,
	[is_active] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[category_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Item_Contact]    Script Date: 3/14/2026 6:19:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Item_Contact](
	[icontact_id] [int] IDENTITY(1,1) NOT NULL,
	[phone_number] [varchar](20) NULL,
	[email] [nvarchar](254) NULL,
PRIMARY KEY CLUSTERED 
(
	[icontact_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Item_Images]    Script Date: 3/14/2026 6:19:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Item_Images](
	[image_id] [int] IDENTITY(1,1) NOT NULL,
	[id_item] [int] NOT NULL,
	[image_url] [nvarchar](256) NULL,
PRIMARY KEY CLUSTERED 
(
	[image_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Item_Requisition]    Script Date: 3/14/2026 6:19:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Item_Requisition](
	[requisition_id] [int] IDENTITY(1,1) NOT NULL,
	[item_variant_id] [int] NOT NULL,
	[id_parent] [int] NOT NULL,
	[quantity] [int] NOT NULL,
	[requested_at] [datetime2](7) NOT NULL,
	[need_from] [datetime2](7) NULL,
	[need_until] [datetime2](7) NULL,
	[expected_return_date] [date] NULL,
	[returned_at] [datetime2](7) NULL,
	[status] [tinyint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[requisition_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Item_Variant]    Script Date: 3/14/2026 6:19:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Item_Variant](
	[variant_id] [int] IDENTITY(1,1) NOT NULL,
	[id_item] [int] NOT NULL,
	[color] [nvarchar](32) NULL,
	[size] [nvarchar](8) NULL,
	[quantity] [int] NOT NULL,
	[price] [decimal](10, 2) NULL,
	[is_active] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[variant_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Modality]    Script Date: 3/14/2026 6:19:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Modality](
	[modality_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](64) NOT NULL,
	[is_active] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[modality_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[News_Post]    Script Date: 3/14/2026 6:19:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[News_Post](
	[post_id] [int] IDENTITY(1,1) NOT NULL,
	[title] [nvarchar](64) NOT NULL,
	[subtitle] [nvarchar](128) NULL,
	[description] [nvarchar](256) NULL,
	[image_url] [nvarchar](256) NULL,
	[created_at] [date] NULL,
	[created_by] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[post_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notification]    Script Date: 3/14/2026 6:19:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notification](
	[notification_id] [int] IDENTITY(1,1) NOT NULL,
	[id_user] [int] NULL,
	[title] [nvarchar](128) NULL,
	[message] [nvarchar](256) NULL,
	[type] [tinyint] NULL,
	[entity_type] [nvarchar](32) NULL,
	[entity_id] [int] NULL,
	[created_at] [datetime2](7) NULL,
	[read_at] [datetime2](7) NULL,
	[is_sent] [bit] NOT NULL,
	[is_deleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[notification_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Participant]    Script Date: 3/14/2026 6:19:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Participant](
	[participant_id] [int] IDENTITY(1,1) NOT NULL,
	[id_coach_class] [int] NOT NULL,
	[id_student] [int] NOT NULL,
	[class_price] [decimal](10, 2) NOT NULL,
	[joined_at] [date] NOT NULL,
	[parent_validated_at] [datetime2](7) NULL,
	[validation_status] [tinyint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[participant_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_ClassStudent] UNIQUE NONCLUSTERED 
(
	[id_coach_class] ASC,
	[id_student] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Person_Info]    Script Date: 3/14/2026 6:19:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Person_Info](
	[person_id] [int] IDENTITY(1,1) NOT NULL,
	[first_name] [nvarchar](64) NOT NULL,
	[last_name] [nvarchar](64) NOT NULL,
	[birth_date] [date] NULL,
	[phone] [varchar](20) NULL,
	[address] [nvarchar](128) NULL,
	[nif] [varchar](9) NULL,
PRIMARY KEY CLUSTERED 
(
	[person_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 3/14/2026 6:19:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[role_id] [tinyint] NOT NULL,
	[role_name] [nvarchar](32) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[role_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[role_name] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Student]    Script Date: 3/14/2026 6:19:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Student](
	[student_id] [int] IDENTITY(1,1) NOT NULL,
	[parent_user_id] [int] NOT NULL,
	[person_info_id] [int] NOT NULL,
	[is_active] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[student_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Studio]    Script Date: 3/14/2026 6:19:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Studio](
	[studio_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](64) NOT NULL,
	[capacity] [int] NOT NULL,
	[address] [nvarchar](256) NULL,
	[is_active] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[studio_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Studio_Modality]    Script Date: 3/14/2026 6:19:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Studio_Modality](
	[id_studio] [int] NOT NULL,
	[id_modality] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id_studio] ASC,
	[id_modality] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 3/14/2026 6:19:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[user_id] [int] IDENTITY(1,1) NOT NULL,
	[username] [nvarchar](64) NOT NULL,
	[password_hash] [nvarchar](256) NOT NULL,
	[is_active] [bit] NOT NULL,
	[created_at] [date] NOT NULL,
	[email] [nvarchar](254) NULL,
	[person_info_id] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[user_id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[username] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User_Role]    Script Date: 3/14/2026 6:19:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User_Role](
	[id_user] [int] NOT NULL,
	[id_role] [tinyint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id_user] ASC,
	[id_role] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Index [IX_CoachClass_Coach_Time]    Script Date: 3/14/2026 6:19:30 PM ******/
CREATE NONCLUSTERED INDEX [IX_CoachClass_Coach_Time] ON [dbo].[Coach_Class]
(
	[id_coach] ASC,
	[start_datetime] ASC,
	[end_datetime] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_CoachClass_Start]    Script Date: 3/14/2026 6:19:30 PM ******/
CREATE NONCLUSTERED INDEX [IX_CoachClass_Start] ON [dbo].[Coach_Class]
(
	[start_datetime] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_CoachClass_Studio_Time]    Script Date: 3/14/2026 6:19:30 PM ******/
CREATE NONCLUSTERED INDEX [IX_CoachClass_Studio_Time] ON [dbo].[Coach_Class]
(
	[id_studio] ASC,
	[start_datetime] ASC,
	[end_datetime] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ItemRequisition_Parent]    Script Date: 3/14/2026 6:19:30 PM ******/
CREATE NONCLUSTERED INDEX [IX_ItemRequisition_Parent] ON [dbo].[Item_Requisition]
(
	[id_parent] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Participant_Class]    Script Date: 3/14/2026 6:19:30 PM ******/
CREATE NONCLUSTERED INDEX [IX_Participant_Class] ON [dbo].[Participant]
(
	[id_coach_class] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Participant_Student]    Script Date: 3/14/2026 6:19:30 PM ******/
CREATE NONCLUSTERED INDEX [IX_Participant_Student] ON [dbo].[Participant]
(
	[id_student] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, DROP_EXISTING = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UX_User_Email]    Script Date: 3/14/2026 6:19:30 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UX_User_Email] ON [dbo].[User]
(
	[email] ASC
)
WHERE ([email] IS NOT NULL)
WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Coach_Class] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[Event] ADD  DEFAULT ((1)) FOR [is_active]
GO
ALTER TABLE [dbo].[Item_Category] ADD  DEFAULT ((1)) FOR [is_active]
GO
ALTER TABLE [dbo].[Item_Variant] ADD  DEFAULT ((1)) FOR [is_active]
GO
ALTER TABLE [dbo].[Modality] ADD  DEFAULT ((1)) FOR [is_active]
GO
ALTER TABLE [dbo].[Notification] ADD  DEFAULT ((0)) FOR [is_deleted]
GO
ALTER TABLE [dbo].[Student] ADD  DEFAULT ((1)) FOR [is_active]
GO
ALTER TABLE [dbo].[Studio] ADD  DEFAULT ((1)) FOR [is_active]
GO
ALTER TABLE [dbo].[User] ADD  DEFAULT ((1)) FOR [is_active]
GO
ALTER TABLE [dbo].[User] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[Blocked_Period]  WITH CHECK ADD FOREIGN KEY([id_coach])
REFERENCES [dbo].[Coach] ([coach_id])
GO
ALTER TABLE [dbo].[Blocked_Period]  WITH CHECK ADD FOREIGN KEY([id_studio])
REFERENCES [dbo].[Studio] ([studio_id])
GO
ALTER TABLE [dbo].[Coach]  WITH CHECK ADD FOREIGN KEY([coach_id])
REFERENCES [dbo].[User] ([user_id])
GO
ALTER TABLE [dbo].[Coach_Availability]  WITH CHECK ADD FOREIGN KEY([id_coach])
REFERENCES [dbo].[Coach] ([coach_id])
GO
ALTER TABLE [dbo].[Coach_Class]  WITH CHECK ADD FOREIGN KEY([created_by])
REFERENCES [dbo].[User] ([user_id])
GO
ALTER TABLE [dbo].[Coach_Class]  WITH CHECK ADD FOREIGN KEY([id_coach])
REFERENCES [dbo].[Coach] ([coach_id])
GO
ALTER TABLE [dbo].[Coach_Class]  WITH CHECK ADD FOREIGN KEY([id_modality])
REFERENCES [dbo].[Modality] ([modality_id])
GO
ALTER TABLE [dbo].[Coach_Class]  WITH CHECK ADD FOREIGN KEY([id_studio])
REFERENCES [dbo].[Studio] ([studio_id])
GO
ALTER TABLE [dbo].[Coach_Modality]  WITH CHECK ADD FOREIGN KEY([id_coach])
REFERENCES [dbo].[Coach] ([coach_id])
GO
ALTER TABLE [dbo].[Coach_Modality]  WITH CHECK ADD FOREIGN KEY([id_modality])
REFERENCES [dbo].[Modality] ([modality_id])
GO
ALTER TABLE [dbo].[Event]  WITH CHECK ADD  CONSTRAINT [FK_Event_User] FOREIGN KEY([created_by])
REFERENCES [dbo].[User] ([user_id])
GO
ALTER TABLE [dbo].[Event] CHECK CONSTRAINT [FK_Event_User]
GO
ALTER TABLE [dbo].[Item]  WITH CHECK ADD FOREIGN KEY([id_category])
REFERENCES [dbo].[Item_Category] ([category_id])
GO
ALTER TABLE [dbo].[Item]  WITH CHECK ADD FOREIGN KEY([id_contact])
REFERENCES [dbo].[Item_Contact] ([icontact_id])
GO
ALTER TABLE [dbo].[Item]  WITH CHECK ADD FOREIGN KEY([id_owner])
REFERENCES [dbo].[User] ([user_id])
GO
ALTER TABLE [dbo].[Item_Images]  WITH CHECK ADD FOREIGN KEY([id_item])
REFERENCES [dbo].[Item] ([item_id])
GO
ALTER TABLE [dbo].[Item_Requisition]  WITH CHECK ADD FOREIGN KEY([id_parent])
REFERENCES [dbo].[User] ([user_id])
GO
ALTER TABLE [dbo].[Item_Requisition]  WITH CHECK ADD FOREIGN KEY([item_variant_id])
REFERENCES [dbo].[Item_Variant] ([variant_id])
GO
ALTER TABLE [dbo].[Item_Variant]  WITH CHECK ADD FOREIGN KEY([id_item])
REFERENCES [dbo].[Item] ([item_id])
GO
ALTER TABLE [dbo].[News_Post]  WITH CHECK ADD  CONSTRAINT [FK_NewsPost_User] FOREIGN KEY([created_by])
REFERENCES [dbo].[User] ([user_id])
GO
ALTER TABLE [dbo].[News_Post] CHECK CONSTRAINT [FK_NewsPost_User]
GO
ALTER TABLE [dbo].[Notification]  WITH CHECK ADD FOREIGN KEY([id_user])
REFERENCES [dbo].[User] ([user_id])
GO
ALTER TABLE [dbo].[Participant]  WITH CHECK ADD FOREIGN KEY([id_coach_class])
REFERENCES [dbo].[Coach_Class] ([class_id])
GO
ALTER TABLE [dbo].[Participant]  WITH CHECK ADD FOREIGN KEY([id_student])
REFERENCES [dbo].[Student] ([student_id])
GO
ALTER TABLE [dbo].[Student]  WITH CHECK ADD  CONSTRAINT [FK_Student_PersonInfo] FOREIGN KEY([person_info_id])
REFERENCES [dbo].[Person_Info] ([person_id])
GO
ALTER TABLE [dbo].[Student] CHECK CONSTRAINT [FK_Student_PersonInfo]
GO
ALTER TABLE [dbo].[Student]  WITH CHECK ADD  CONSTRAINT [FK_Student_UserParent] FOREIGN KEY([parent_user_id])
REFERENCES [dbo].[User] ([user_id])
GO
ALTER TABLE [dbo].[Student] CHECK CONSTRAINT [FK_Student_UserParent]
GO
ALTER TABLE [dbo].[Studio_Modality]  WITH CHECK ADD FOREIGN KEY([id_modality])
REFERENCES [dbo].[Modality] ([modality_id])
GO
ALTER TABLE [dbo].[Studio_Modality]  WITH CHECK ADD FOREIGN KEY([id_studio])
REFERENCES [dbo].[Studio] ([studio_id])
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_PersonInfo] FOREIGN KEY([person_info_id])
REFERENCES [dbo].[Person_Info] ([person_id])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_PersonInfo]
GO
ALTER TABLE [dbo].[User_Role]  WITH CHECK ADD FOREIGN KEY([id_role])
REFERENCES [dbo].[Role] ([role_id])
GO
ALTER TABLE [dbo].[User_Role]  WITH CHECK ADD FOREIGN KEY([id_user])
REFERENCES [dbo].[User] ([user_id])
GO
ALTER TABLE [dbo].[Blocked_Period]  WITH CHECK ADD  CONSTRAINT [CK_BlockedPeriod_Scope] CHECK  (([scope]=(3) OR [scope]=(2) OR [scope]=(1)))
GO
ALTER TABLE [dbo].[Blocked_Period] CHECK CONSTRAINT [CK_BlockedPeriod_Scope]
GO
ALTER DATABASE [DanceSchoolDb] SET  READ_WRITE 
GO
