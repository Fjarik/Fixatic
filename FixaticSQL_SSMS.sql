/*    ==Scripting Parameters==

    Source Server Version : SQL Server 2019 (15.0.2080)
    Source Database Engine Edition : Microsoft SQL Server Enterprise Edition
    Source Database Engine Type : Standalone SQL Server

    Target Server Version : SQL Server 2019
    Target Database Engine Edition : Microsoft SQL Server Enterprise Edition
    Target Database Engine Type : Standalone SQL Server
*/

/****** Object:  Table [dbo].[Attachements]    Script Date: 20.03.2022 12:16:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Attachements]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Attachements](
	[Attachement_ID] [int] IDENTITY(1,1) NOT NULL,
	[User_ID] [int] NOT NULL,
	[Comment_ID] [int] NULL,
	[Ticket_ID] [int] NULL,
	[Content] [varbinary](max) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Size] [int] NOT NULL,
	[Type] [varchar](50) NOT NULL,
	[Uploaded] [datetime2](7) NOT NULL,
	[AlternativeText] [nvarchar](250) NULL,
 CONSTRAINT [PK_Attachements] PRIMARY KEY CLUSTERED 
(
	[Attachement_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 20.03.2022 12:16:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Categories]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Categories](
	[Category_ID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](250) NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
(
	[Category_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Comments]    Script Date: 20.03.2022 12:16:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Comments]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Comments](
	[Comment_ID] [int] IDENTITY(1,1) NOT NULL,
	[Ticket_ID] [int] NOT NULL,
	[User_ID] [int] NOT NULL,
	[Content] [ntext] NOT NULL,
	[Created] [datetime2](7) NOT NULL,
	[IsInternal] [bit] NOT NULL,
 CONSTRAINT [PK_Comments] PRIMARY KEY CLUSTERED 
(
	[Comment_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[CustomProperties]    Script Date: 20.03.2022 12:16:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CustomProperties]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CustomProperties](
	[CustomProperty_ID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](250) NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_CustomProperties] PRIMARY KEY CLUSTERED 
(
	[CustomProperty_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[CustomPropertyOptions]    Script Date: 20.03.2022 12:16:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CustomPropertyOptions]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CustomPropertyOptions](
	[CustomPropertyOption_ID] [int] IDENTITY(1,1) NOT NULL,
	[CustomProperty_ID] [int] NOT NULL,
	[Content] [ntext] NOT NULL,
	[IsEnabled] [bit] NOT NULL,
	[Sequence] [int] NOT NULL,
 CONSTRAINT [PK_CustomPropertyOptions] PRIMARY KEY CLUSTERED 
(
	[CustomPropertyOption_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[CustomPropertyValues]    Script Date: 20.03.2022 12:16:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CustomPropertyValues]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CustomPropertyValues](
	[CustomPropertyOption_ID] [int] NOT NULL,
	[Created] [datetime2](7) NOT NULL,
	[Ticket_ID] [int] NOT NULL,
 CONSTRAINT [PK_CustomPropertyValues] PRIMARY KEY CLUSTERED 
(
	[CustomPropertyOption_ID] ASC,
	[Ticket_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Followers]    Script Date: 20.03.2022 12:16:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Followers]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Followers](
	[User_ID] [int] NOT NULL,
	[Ticket_ID] [int] NOT NULL,
	[Since] [datetime2](7) NOT NULL,
	[Type] [int] NOT NULL,
 CONSTRAINT [PK_Followers] PRIMARY KEY CLUSTERED 
(
	[User_ID] ASC,
	[Ticket_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Groups]    Script Date: 20.03.2022 12:16:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Groups]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Groups](
	[Group_ID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](250) NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Type] [int] NOT NULL,
 CONSTRAINT [PK_Groups] PRIMARY KEY CLUSTERED 
(
	[Group_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Projects]    Script Date: 20.03.2022 12:16:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Projects]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Projects](
	[Project_ID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](250) NULL,
	[IsEnabled] [bit] NOT NULL,
	[IsInternal] [bit] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Shortcut] [nvarchar](4) NOT NULL,
 CONSTRAINT [PK_Projects] PRIMARY KEY CLUSTERED 
(
	[Project_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[ProjectsAccess]    Script Date: 20.03.2022 12:16:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProjectsAccess]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ProjectsAccess](
	[Project_ID] [int] NOT NULL,
	[Group_ID] [int] NOT NULL,
 CONSTRAINT [PK_ProjectsAccess] PRIMARY KEY CLUSTERED 
(
	[Project_ID] ASC,
	[Group_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[ProjectsCategories]    Script Date: 20.03.2022 12:16:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProjectsCategories]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ProjectsCategories](
	[Category_ID] [int] NOT NULL,
	[Project_ID] [int] NOT NULL,
 CONSTRAINT [PK_ProjectsCategories] PRIMARY KEY CLUSTERED 
(
	[Category_ID] ASC,
	[Project_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Tickets]    Script Date: 20.03.2022 12:16:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Tickets]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Tickets](
	[Ticket_ID] [int] IDENTITY(1,1) NOT NULL,
	[Project_ID] [int] NOT NULL,
	[AssignedUser_ID] [int] NULL,
	[Creator_ID] [int] NOT NULL,
	[Content] [ntext] NOT NULL,
	[Created] [datetime2](7) NOT NULL,
	[DateSolved] [datetime2](7) NULL,
	[Modified] [datetime2](7) NULL,
	[Priority] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
	[Type] [int] NOT NULL,
	[Visibility] [int] NOT NULL,
 CONSTRAINT [PK_Tickets] PRIMARY KEY CLUSTERED 
(
	[Ticket_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Users]    Script Date: 20.03.2022 12:16:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Users](
	[User_ID] [int] IDENTITY(1,1) NOT NULL,
	[Created] [datetime2](7) NOT NULL,
	[IsEnabled] [bit] NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[Firstname] [nvarchar](50) NOT NULL,
	[Lastname] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](250) NOT NULL,
	[Phone] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[User_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[UsersGroups]    Script Date: 20.03.2022 12:16:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UsersGroups]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[UsersGroups](
	[Group_ID] [int] NOT NULL,
	[User_ID] [int] NOT NULL,
 CONSTRAINT [PK_UsersGroups] PRIMARY KEY CLUSTERED 
(
	[Group_ID] ASC,
	[User_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET IDENTITY_INSERT [dbo].[Categories] ON 
GO
INSERT [dbo].[Categories] ([Description], [Name], [Category_ID]) VALUES (N'All games we provide', N'Games', 1)
GO
INSERT [dbo].[Categories] ([Description], [Name], [Category_ID]) VALUES (N'Web services or applications', N'Websites', 2)
GO
INSERT [dbo].[Categories] ([Description], [Name], [Category_ID]) VALUES (N'The applications', N'Applications', 3)
GO
SET IDENTITY_INSERT [dbo].[Categories] OFF
GO
SET IDENTITY_INSERT [dbo].[Comments] ON 
GO
INSERT [dbo].[Comments] ([Content], [Created], [IsInternal], [Comment_ID], [Ticket_ID], [User_ID]) VALUES (N'Can you provide us with more information?', CAST(N'2019-02-26T11:19:21.0000000' AS DateTime2), 1, 1, 1, 4)
GO
INSERT [dbo].[Comments] ([Content], [Created], [IsInternal], [Comment_ID], [Ticket_ID], [User_ID]) VALUES (N'We are currently working on your problem', CAST(N'2020-10-04T12:10:05.0000000' AS DateTime2), 1, 2, 5, 3)
GO
INSERT [dbo].[Comments] ([Content], [Created], [IsInternal], [Comment_ID], [Ticket_ID], [User_ID]) VALUES (N'Try to update your application first', CAST(N'2020-10-04T09:54:01.0000000' AS DateTime2), 1, 3, 2, 4)
GO
INSERT [dbo].[Comments] ([Content], [Created], [IsInternal], [Comment_ID], [Ticket_ID], [User_ID]) VALUES (N'It says that I have the latest version', CAST(N'2020-10-04T12:12:06.0000000' AS DateTime2), 0, 4, 2, 3)
GO
SET IDENTITY_INSERT [dbo].[Comments] OFF
GO
INSERT [dbo].[CustomProperties] ([Description], [Name], [CustomProperty_ID]) VALUES (N'Label for project', N'Label', 1)
GO
INSERT [dbo].[CustomProperties] ([Description], [Name], [CustomProperty_ID]) VALUES (N'Affected version', N'Version', 2)
GO
SET IDENTITY_INSERT [dbo].[CustomPropertyOptions] ON 
GO
INSERT [dbo].[CustomPropertyOptions] ([Content], [IsEnabled], [Sequence], [CustomPropertyOption_ID], [CustomProperty_ID]) VALUES (N'A', 1, 1, 1, 1)
GO
INSERT [dbo].[CustomPropertyOptions] ([Content], [IsEnabled], [Sequence], [CustomPropertyOption_ID], [CustomProperty_ID]) VALUES (N'B', 1, 2, 2, 1)
GO
INSERT [dbo].[CustomPropertyOptions] ([Content], [IsEnabled], [Sequence], [CustomPropertyOption_ID], [CustomProperty_ID]) VALUES (N'C', 1, 3, 3, 1)
GO
INSERT [dbo].[CustomPropertyOptions] ([Content], [IsEnabled], [Sequence], [CustomPropertyOption_ID], [CustomProperty_ID]) VALUES (N'D', 1, 4, 4, 1)
GO
INSERT [dbo].[CustomPropertyOptions] ([Content], [IsEnabled], [Sequence], [CustomPropertyOption_ID], [CustomProperty_ID]) VALUES (N'1.0.0', 1, 1, 5, 2)
GO
INSERT [dbo].[CustomPropertyOptions] ([Content], [IsEnabled], [Sequence], [CustomPropertyOption_ID], [CustomProperty_ID]) VALUES (N'1.1.0', 1, 2, 6, 2)
GO
INSERT [dbo].[CustomPropertyOptions] ([Content], [IsEnabled], [Sequence], [CustomPropertyOption_ID], [CustomProperty_ID]) VALUES (N'1.2.0', 1, 3, 7, 2)
GO
INSERT [dbo].[CustomPropertyOptions] ([Content], [IsEnabled], [Sequence], [CustomPropertyOption_ID], [CustomProperty_ID]) VALUES (N'1.3.0', 1, 4, 8, 2)
GO
SET IDENTITY_INSERT [dbo].[CustomPropertyOptions] OFF
GO
INSERT [dbo].[CustomPropertyValues] ([Created], [CustomPropertyOption_ID], [Ticket_ID]) VALUES (CAST(N'2018-08-30T17:17:22.0000000' AS DateTime2), 1, 4)
GO
INSERT [dbo].[CustomPropertyValues] ([Created], [CustomPropertyOption_ID], [Ticket_ID]) VALUES (CAST(N'2018-08-30T17:17:22.0000000' AS DateTime2), 2, 4)
GO
INSERT [dbo].[CustomPropertyValues] ([Created], [CustomPropertyOption_ID], [Ticket_ID]) VALUES (CAST(N'2018-08-30T17:17:22.0000000' AS DateTime2), 5, 4)
GO
INSERT [dbo].[CustomPropertyValues] ([Created], [CustomPropertyOption_ID], [Ticket_ID]) VALUES (CAST(N'2018-08-30T17:17:22.0000000' AS DateTime2), 6, 4)
GO
INSERT [dbo].[CustomPropertyValues] ([Created], [CustomPropertyOption_ID], [Ticket_ID]) VALUES (CAST(N'2018-08-30T17:17:22.0000000' AS DateTime2), 7, 4)
GO
INSERT [dbo].[CustomPropertyValues] ([Created], [CustomPropertyOption_ID], [Ticket_ID]) VALUES (CAST(N'2019-04-18T08:01:45.0000000' AS DateTime2), 4, 10)
GO
INSERT [dbo].[CustomPropertyValues] ([Created], [CustomPropertyOption_ID], [Ticket_ID]) VALUES (CAST(N'2019-04-12T08:42:45.0000000' AS DateTime2), 6, 2)
GO
INSERT [dbo].[CustomPropertyValues] ([Created], [CustomPropertyOption_ID], [Ticket_ID]) VALUES (CAST(N'2020-10-06T12:34:54.0000000' AS DateTime2), 7, 8)
GO
INSERT [dbo].[CustomPropertyValues] ([Created], [CustomPropertyOption_ID], [Ticket_ID]) VALUES (CAST(N'2021-02-15T17:18:20.0000000' AS DateTime2), 8, 9)
GO
INSERT [dbo].[Followers] ([Since], [Type], [User_ID], [Ticket_ID]) VALUES (CAST(N'2019-02-26T07:15:24.0000000' AS DateTime2), 1, 10, 1)
GO
INSERT [dbo].[Followers] ([Since], [Type], [User_ID], [Ticket_ID]) VALUES (CAST(N'2020-10-04T20:24:24.0000000' AS DateTime2), 1, 10, 5)
GO
INSERT [dbo].[Followers] ([Since], [Type], [User_ID], [Ticket_ID]) VALUES (CAST(N'2020-10-05T21:54:41.0000000' AS DateTime2), 1, 11, 5)
GO
INSERT [dbo].[Followers] ([Since], [Type], [User_ID], [Ticket_ID]) VALUES (CAST(N'2019-04-08T11:56:01.0000000' AS DateTime2), 1, 11, 7)
GO
INSERT [dbo].[Followers] ([Since], [Type], [User_ID], [Ticket_ID]) VALUES (CAST(N'2019-04-07T06:50:14.0000000' AS DateTime2), 1, 12, 7)
GO
INSERT [dbo].[Followers] ([Since], [Type], [User_ID], [Ticket_ID]) VALUES (CAST(N'2019-08-17T04:59:10.0000000' AS DateTime2), 1, 13, 3)
GO
SET IDENTITY_INSERT [dbo].[Groups] ON 
GO
INSERT [dbo].[Groups] ([Description], [Name], [Type], [Group_ID]) VALUES (N'The group developing the software', N'Developers', 2, 1)
GO
INSERT [dbo].[Groups] ([Description], [Name], [Type], [Group_ID]) VALUES (N'The group testing the software', N'Testers', 2, 2)
GO
INSERT [dbo].[Groups] ([Description], [Name], [Type], [Group_ID]) VALUES (N'The group for the project managers', N'Project managers', 2, 3)
GO
INSERT [dbo].[Groups] ([Description], [Name], [Type], [Group_ID]) VALUES (N'The group of the external company developing the software ', N'Microsoft Developers', 4, 4)
GO
INSERT [dbo].[Groups] ([Description], [Name], [Type], [Group_ID]) VALUES (N'The group of the external company testing the software', N'Microsoft Testers', 4, 5)
GO
INSERT [dbo].[Groups] ([Description], [Name], [Type], [Group_ID]) VALUES (N'The group of administrators', N'Admins', 8, 6)
GO
SET IDENTITY_INSERT [dbo].[Groups] OFF
GO
SET IDENTITY_INSERT [dbo].[Projects] ON 
GO
INSERT [dbo].[Projects] ([Description], [IsEnabled], [IsInternal], [Name], [Shortcut], [Project_ID]) VALUES (N'The C++ version on Minecraft', 1, 1, N'Minecraft CPP Edition', N'MCCP', 1)
GO
INSERT [dbo].[Projects] ([Description], [IsEnabled], [IsInternal], [Name], [Shortcut], [Project_ID]) VALUES (N'The social media android app for small groups', 1, 1, N'BookFace', N'BF', 2)
GO
INSERT [dbo].[Projects] ([Description], [IsEnabled], [IsInternal], [Name], [Shortcut], [Project_ID]) VALUES (N'The app for uploading videos', 1, 0, N'TubeYou', N'TY', 3)
GO
INSERT [dbo].[Projects] ([Description], [IsEnabled], [IsInternal], [Name], [Shortcut], [Project_ID]) VALUES (N'The workout app', 1, 1, N'Sigma Grind', N'SiGr', 4)
GO
INSERT [dbo].[Projects] ([Description], [IsEnabled], [IsInternal], [Name], [Shortcut], [Project_ID]) VALUES (N'The app for internet discusion', 0, 0, N'Tidder', N'TDDR', 5)
GO
INSERT [dbo].[Projects] ([Description], [IsEnabled], [IsInternal], [Name], [Shortcut], [Project_ID]) VALUES (N'The app for playing and downloading music', 0, 1, N'Musify', N'MSF', 6)
GO
INSERT [dbo].[Projects] ([Description], [IsEnabled], [IsInternal], [Name], [Shortcut], [Project_ID]) VALUES (N'Another DLC to The Sims 4', 1, 1, N'The Sims city expansion', N'TSCE', 7)
GO
INSERT [dbo].[Projects] ([Description], [IsEnabled], [IsInternal], [Name], [Shortcut], [Project_ID]) VALUES (N'The race game', 1, 0, N'Speed is needed', N'SIN', 8)
GO
INSERT [dbo].[Projects] ([Description], [IsEnabled], [IsInternal], [Name], [Shortcut], [Project_ID]) VALUES (N'The race and derby game', 1, 0, N'Out Flat 2', N'OF2', 9)
GO
INSERT [dbo].[Projects] ([Description], [IsEnabled], [IsInternal], [Name], [Shortcut], [Project_ID]) VALUES (N'The third episode of Half-Life', 0, 0, N'Half-Life 3', N'HF3', 10)
GO
INSERT [dbo].[Projects] ([Description], [IsEnabled], [IsInternal], [Name], [Shortcut], [Project_ID]) VALUES (N'Dark Souls 2 DLC', 1, 0, N'Dark Souls 2: Electric Boogaloo', N'DS2E', 11)
GO
INSERT [dbo].[Projects] ([Description], [IsEnabled], [IsInternal], [Name], [Shortcut], [Project_ID]) VALUES (N'Visual modeling and design tool based on the OMG UML', 1, 1, N'Architect of Enterprises', N'AOE', 12)
GO
INSERT [dbo].[Projects] ([Description], [IsEnabled], [IsInternal], [Name], [Shortcut], [Project_ID]) VALUES (N'The app for workouts', 1, 1, N'Exercises are not futile', N'EANF', 13)
GO
SET IDENTITY_INSERT [dbo].[Projects] OFF
GO
INSERT [dbo].[ProjectsAccess] ([Project_ID], [Group_ID]) VALUES (1, 1)
GO
INSERT [dbo].[ProjectsAccess] ([Project_ID], [Group_ID]) VALUES (2, 1)
GO
INSERT [dbo].[ProjectsAccess] ([Project_ID], [Group_ID]) VALUES (3, 1)
GO
INSERT [dbo].[ProjectsAccess] ([Project_ID], [Group_ID]) VALUES (4, 1)
GO
INSERT [dbo].[ProjectsAccess] ([Project_ID], [Group_ID]) VALUES (5, 1)
GO
INSERT [dbo].[ProjectsAccess] ([Project_ID], [Group_ID]) VALUES (6, 1)
GO
INSERT [dbo].[ProjectsAccess] ([Project_ID], [Group_ID]) VALUES (7, 1)
GO
INSERT [dbo].[ProjectsAccess] ([Project_ID], [Group_ID]) VALUES (8, 1)
GO
INSERT [dbo].[ProjectsAccess] ([Project_ID], [Group_ID]) VALUES (9, 1)
GO
INSERT [dbo].[ProjectsAccess] ([Project_ID], [Group_ID]) VALUES (10, 1)
GO
INSERT [dbo].[ProjectsAccess] ([Project_ID], [Group_ID]) VALUES (11, 1)
GO
INSERT [dbo].[ProjectsAccess] ([Project_ID], [Group_ID]) VALUES (12, 1)
GO
INSERT [dbo].[ProjectsAccess] ([Project_ID], [Group_ID]) VALUES (13, 1)
GO

INSERT [dbo].[ProjectsAccess] ([Project_ID], [Group_ID]) VALUES (1, 2)
GO
INSERT [dbo].[ProjectsAccess] ([Project_ID], [Group_ID]) VALUES (2, 2)
GO
INSERT [dbo].[ProjectsAccess] ([Project_ID], [Group_ID]) VALUES (3, 2)
GO
INSERT [dbo].[ProjectsAccess] ([Project_ID], [Group_ID]) VALUES (4, 2)
GO
INSERT [dbo].[ProjectsAccess] ([Project_ID], [Group_ID]) VALUES (5, 2)
GO
INSERT [dbo].[ProjectsAccess] ([Project_ID], [Group_ID]) VALUES (6, 2)
GO
INSERT [dbo].[ProjectsAccess] ([Project_ID], [Group_ID]) VALUES (7, 2)
GO
INSERT [dbo].[ProjectsAccess] ([Project_ID], [Group_ID]) VALUES (8, 2)
GO
INSERT [dbo].[ProjectsAccess] ([Project_ID], [Group_ID]) VALUES (9, 2)
GO
INSERT [dbo].[ProjectsAccess] ([Project_ID], [Group_ID]) VALUES (10, 2)
GO
INSERT [dbo].[ProjectsAccess] ([Project_ID], [Group_ID]) VALUES (11, 2)
GO
INSERT [dbo].[ProjectsAccess] ([Project_ID], [Group_ID]) VALUES (12, 2)
GO
INSERT [dbo].[ProjectsAccess] ([Project_ID], [Group_ID]) VALUES (13, 2)
GO

INSERT [dbo].[ProjectsAccess] ([Project_ID], [Group_ID]) VALUES (1, 3)
GO
INSERT [dbo].[ProjectsAccess] ([Project_ID], [Group_ID]) VALUES (2, 3)
GO
INSERT [dbo].[ProjectsAccess] ([Project_ID], [Group_ID]) VALUES (3, 3)
GO
INSERT [dbo].[ProjectsAccess] ([Project_ID], [Group_ID]) VALUES (4, 3)
GO
INSERT [dbo].[ProjectsAccess] ([Project_ID], [Group_ID]) VALUES (5, 3)
GO
INSERT [dbo].[ProjectsAccess] ([Project_ID], [Group_ID]) VALUES (6, 3)
GO
INSERT [dbo].[ProjectsAccess] ([Project_ID], [Group_ID]) VALUES (7, 3)
GO
INSERT [dbo].[ProjectsAccess] ([Project_ID], [Group_ID]) VALUES (8, 3)
GO
INSERT [dbo].[ProjectsAccess] ([Project_ID], [Group_ID]) VALUES (9, 3)
GO
INSERT [dbo].[ProjectsAccess] ([Project_ID], [Group_ID]) VALUES (10, 3)
GO
INSERT [dbo].[ProjectsAccess] ([Project_ID], [Group_ID]) VALUES (11, 3)
GO
INSERT [dbo].[ProjectsAccess] ([Project_ID], [Group_ID]) VALUES (12, 3)
GO
INSERT [dbo].[ProjectsAccess] ([Project_ID], [Group_ID]) VALUES (13, 3)
GO

INSERT [dbo].[ProjectsAccess] ([Project_ID], [Group_ID]) VALUES (1, 4)
GO
INSERT [dbo].[ProjectsAccess] ([Project_ID], [Group_ID]) VALUES (1, 5)
GO
INSERT [dbo].[ProjectsAccess] ([Project_ID], [Group_ID]) VALUES (1, 6)
GO

INSERT [dbo].[ProjectsAccess] ([Project_ID], [Group_ID]) VALUES (8, 4)
GO
INSERT [dbo].[ProjectsAccess] ([Project_ID], [Group_ID]) VALUES (8, 5)
GO
INSERT [dbo].[ProjectsAccess] ([Project_ID], [Group_ID]) VALUES (11, 5)
GO



INSERT [dbo].[ProjectsCategories] ([Category_ID], [Project_ID]) VALUES (1, 1)
GO
INSERT [dbo].[ProjectsCategories] ([Category_ID], [Project_ID]) VALUES (1, 7)
GO
INSERT [dbo].[ProjectsCategories] ([Category_ID], [Project_ID]) VALUES (1, 8)
GO
INSERT [dbo].[ProjectsCategories] ([Category_ID], [Project_ID]) VALUES (1, 9)
GO
INSERT [dbo].[ProjectsCategories] ([Category_ID], [Project_ID]) VALUES (1, 10)
GO

INSERT [dbo].[ProjectsCategories] ([Category_ID], [Project_ID]) VALUES (2, 2)
GO
INSERT [dbo].[ProjectsCategories] ([Category_ID], [Project_ID]) VALUES (2, 3)
GO
INSERT [dbo].[ProjectsCategories] ([Category_ID], [Project_ID]) VALUES (2, 4)
GO
INSERT [dbo].[ProjectsCategories] ([Category_ID], [Project_ID]) VALUES (2, 5)
GO
INSERT [dbo].[ProjectsCategories] ([Category_ID], [Project_ID]) VALUES (2, 6)
GO

INSERT [dbo].[ProjectsCategories] ([Category_ID], [Project_ID]) VALUES (3, 4)
GO
INSERT [dbo].[ProjectsCategories] ([Category_ID], [Project_ID]) VALUES (3, 6)
GO
INSERT [dbo].[ProjectsCategories] ([Category_ID], [Project_ID]) VALUES (3, 12)
GO
INSERT [dbo].[ProjectsCategories] ([Category_ID], [Project_ID]) VALUES (3, 13)
GO



SET IDENTITY_INSERT [dbo].[Tickets] ON 
GO
INSERT [dbo].[Tickets] ([Content], [Created], [DateSolved], [Modified], [Priority], [Status], [Title], [Type], [Visibility], [Ticket_ID], [Project_ID], [AssignedUser_ID], [Creator_ID]) VALUES (N'The rendering is not working properly when walking at night', CAST(N'2019-02-26T05:09:24.0000000' AS DateTime2), NULL, NULL, 54, 0, N'Minecraft rendering bug', 1, 1, 1, 1, 2, 12)
GO
INSERT [dbo].[Tickets] ([Content], [Created], [DateSolved], [Modified], [Priority], [Status], [Title], [Type], [Visibility], [Ticket_ID], [Project_ID], [AssignedUser_ID], [Creator_ID]) VALUES (N'Chat does not load and then the app crashes', CAST(N'2019-04-08T05:42:59.0000000' AS DateTime2), CAST(N'2019-04-12T08:42:45.0000000' AS DateTime2), NULL, 91, 1, N'BookFace chat load bug', 1, 1, 2, 2, 10, 13)
GO
INSERT [dbo].[Tickets] ([Content], [Created], [DateSolved], [Modified], [Priority], [Status], [Title], [Type], [Visibility], [Ticket_ID], [Project_ID], [AssignedUser_ID], [Creator_ID]) VALUES (N'Upload button does not work', CAST(N'2019-08-16T07:08:47.0000000' AS DateTime2), NULL, CAST(N'2019-08-16T12:05:44.0000000' AS DateTime2), 88, 1, N'TubeYou button is not working', 2, 1, 3, 3, 6, 14)
GO
INSERT [dbo].[Tickets] ([Content], [Created], [DateSolved], [Modified], [Priority], [Status], [Title], [Type], [Visibility], [Ticket_ID], [Project_ID], [AssignedUser_ID], [Creator_ID]) VALUES (N'Creepers are not spawning', CAST(N'2018-08-27T08:17:49.0000000' AS DateTime2), CAST(N'2018-08-30T17:17:22.0000000' AS DateTime2), CAST(N'2018-08-27T15:20:41.0000000' AS DateTime2), 71, 1, N'Minecraft creeper spawn bug', 1, 1, 4, 1, 3, 4)
GO
INSERT [dbo].[Tickets] ([Content], [Created], [DateSolved], [Modified], [Priority], [Status], [Title], [Type], [Visibility], [Ticket_ID], [Project_ID], [AssignedUser_ID], [Creator_ID]) VALUES (N'The app crashes when uploading images bigger than 3MB', CAST(N'2020-10-04T05:32:41.0000000' AS DateTime2), NULL, NULL, 24, 0, N'Tidder image upload bug', 1, 2, 5, 5, 9, 5)
GO
INSERT [dbo].[Tickets] ([Content], [Created], [DateSolved], [Modified], [Priority], [Status], [Title], [Type], [Visibility], [Ticket_ID], [Project_ID], [AssignedUser_ID], [Creator_ID]) VALUES (N'The code generator does not generates code for all the selected items', CAST(N'2021-02-09T10:51:54.0000000' AS DateTime2), CAST(N'2021-02-27T19:25:50.0000000' AS DateTime2), NULL, 45, 0, N'Architect of Enterprises code generator bug', 0, 1, 6, 12, 2, 8)
GO
INSERT [dbo].[Tickets] ([Content], [Created], [DateSolved], [Modified], [Priority], [Status], [Title], [Type], [Visibility], [Ticket_ID], [Project_ID], [AssignedUser_ID], [Creator_ID]) VALUES (N'Cannot download more than one song at the time but I should be able', CAST(N'2019-04-07T01:25:10.0000000' AS DateTime2), NULL, NULL, 36, 0, N'Musify download bug', 1, 1, 7, 6, 1, 7)
GO
INSERT [dbo].[Tickets] ([Content], [Created], [DateSolved], [Modified], [Priority], [Status], [Title], [Type], [Visibility], [Ticket_ID], [Project_ID], [AssignedUser_ID], [Creator_ID]) VALUES (N'The profiles does not load when on Android', CAST(N'2020-10-04T05:32:41.0000000' AS DateTime2), CAST(N'2020-10-06T12:34:54.0000000' AS DateTime2), CAST(N'2020-10-04T05:47:01.0000000' AS DateTime2), 79, 1, N'BF profiles does not load', 1, 1, 8, 2, 2, 7)
GO
INSERT [dbo].[Tickets] ([Content], [Created], [DateSolved], [Modified], [Priority], [Status], [Title], [Type], [Visibility], [Ticket_ID], [Project_ID], [AssignedUser_ID], [Creator_ID]) VALUES (N'The game crashes when the cars crash', CAST(N'2021-02-09T10:51:54.0000000' AS DateTime2), CAST(N'2021-02-15T17:18:20.0000000' AS DateTime2), NULL, 81, 0, N'Out Flat 2 car crash bug', 2, 2, 9, 9, 9, 4)
GO
INSERT [dbo].[Tickets] ([Content], [Created], [DateSolved], [Modified], [Priority], [Status], [Title], [Type], [Visibility], [Ticket_ID], [Project_ID], [AssignedUser_ID], [Creator_ID]) VALUES (N'The game does not start when using the DLC', CAST(N'2019-04-14T09:16:23.0000000' AS DateTime2), CAST(N'2019-04-18T08:01:45.0000000' AS DateTime2), CAST(N'2019-04-14T11:23:24.0000000' AS DateTime2), 90, 0, N'DS2:EB does not start', 2, 1, 10, 11, 8, 6)
GO
SET IDENTITY_INSERT [dbo].[Tickets] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 
GO
INSERT [dbo].[Users] ([Created], [IsEnabled], [Email], [Firstname], [Lastname], [Password], [Phone], [User_ID]) VALUES (CAST(N'2020-02-23T06:41:06.0000000' AS DateTime2), 1, N'montes.nascetur@hotmail.net', N'Lynn', N'Knapp', N'AQAAAAEAACcQAAAAEDiDMNsEk8orl8j2ZIlL0DF6FZkEbXPIfenbAPDYY9J61b/oBRNbA8v1pGEVN8CnQA==', N'+420543625426', 1)
GO
INSERT [dbo].[Users] ([Created], [IsEnabled], [Email], [Firstname], [Lastname], [Password], [Phone], [User_ID]) VALUES (CAST(N'2018-03-04T05:22:40.0000000' AS DateTime2), 1, N'arcu.morbi@yahoo.edu', N'Rowan', N'Warner', N'AQAAAAEAACcQAAAAEDiDMNsEk8orl8j2ZIlL0DF6FZkEbXPIfenbAPDYY9J61b/oBRNbA8v1pGEVN8CnQA==', N'+420470486307', 2)
GO
INSERT [dbo].[Users] ([Created], [IsEnabled], [Email], [Firstname], [Lastname], [Password], [Phone], [User_ID]) VALUES (CAST(N'2019-12-15T03:29:56.0000000' AS DateTime2), 0, N'arcu.nunc@hotmail.org', N'Zelda', N'Stout', N'AQAAAAEAACcQAAAAEDiDMNsEk8orl8j2ZIlL0DF6FZkEbXPIfenbAPDYY9J61b/oBRNbA8v1pGEVN8CnQA==', N'+420610886466', 3)
GO
INSERT [dbo].[Users] ([Created], [IsEnabled], [Email], [Firstname], [Lastname], [Password], [Phone], [User_ID]) VALUES (CAST(N'2021-03-11T07:03:33.0000000' AS DateTime2), 1, N'duis.dignissim.tempor@icloud.net', N'Colorado', N'Nevermind', N'AQAAAAEAACcQAAAAEDiDMNsEk8orl8j2ZIlL0DF6FZkEbXPIfenbAPDYY9J61b/oBRNbA8v1pGEVN8CnQA==', N'+420475958417', 4)
GO
INSERT [dbo].[Users] ([Created], [IsEnabled], [Email], [Firstname], [Lastname], [Password], [Phone], [User_ID]) VALUES (CAST(N'2021-01-11T07:38:02.0000000' AS DateTime2), 0, N'egestas.fusce@outlook.com', N'Ruby', N'Medina', N'AQAAAAEAACcQAAAAEDiDMNsEk8orl8j2ZIlL0DF6FZkEbXPIfenbAPDYY9J61b/oBRNbA8v1pGEVN8CnQA==', N'+420716795448', 5)
GO
INSERT [dbo].[Users] ([Created], [IsEnabled], [Email], [Firstname], [Lastname], [Password], [Phone], [User_ID]) VALUES (CAST(N'2019-09-11T11:08:56.0000000' AS DateTime2), 0, N'id.magna@yahoo.org', N'Dieter', N'Blair', N'AQAAAAEAACcQAAAAEDiDMNsEk8orl8j2ZIlL0DF6FZkEbXPIfenbAPDYY9J61b/oBRNbA8v1pGEVN8CnQA==', N'+420814412672', 6)
GO
INSERT [dbo].[Users] ([Created], [IsEnabled], [Email], [Firstname], [Lastname], [Password], [Phone], [User_ID]) VALUES (CAST(N'2020-05-26T08:07:25.0000000' AS DateTime2), 1, N'mi.ac@hotmail.couk', N'Kibo', N'Henderson', N'AQAAAAEAACcQAAAAEDiDMNsEk8orl8j2ZIlL0DF6FZkEbXPIfenbAPDYY9J61b/oBRNbA8v1pGEVN8CnQA==', N'+420965476091', 7)
GO
INSERT [dbo].[Users] ([Created], [IsEnabled], [Email], [Firstname], [Lastname], [Password], [Phone], [User_ID]) VALUES (CAST(N'2019-10-19T08:28:30.0000000' AS DateTime2), 0, N'semper@outlook.edu', N'Lavinia', N'Mcneil', N'AQAAAAEAACcQAAAAEDiDMNsEk8orl8j2ZIlL0DF6FZkEbXPIfenbAPDYY9J61b/oBRNbA8v1pGEVN8CnQA==', N'+420711811071', 8)
GO
INSERT [dbo].[Users] ([Created], [IsEnabled], [Email], [Firstname], [Lastname], [Password], [Phone], [User_ID]) VALUES (CAST(N'2018-10-12T01:48:58.0000000' AS DateTime2), 1, N'bibendum.sed.est@protonmail.com', N'Drake', N'Sharp', N'AQAAAAEAACcQAAAAEDiDMNsEk8orl8j2ZIlL0DF6FZkEbXPIfenbAPDYY9J61b/oBRNbA8v1pGEVN8CnQA==', N'+420764419182', 9)
GO
INSERT [dbo].[Users] ([Created], [IsEnabled], [Email], [Firstname], [Lastname], [Password], [Phone], [User_ID]) VALUES (CAST(N'2018-08-23T02:15:40.0000000' AS DateTime2), 1, N'turpis.aliquam@google.net', N'Keegan', N'Hunter', N'AQAAAAEAACcQAAAAEDiDMNsEk8orl8j2ZIlL0DF6FZkEbXPIfenbAPDYY9J61b/oBRNbA8v1pGEVN8CnQA==', N'+420740495809', 10)
GO
INSERT [dbo].[Users] ([Created], [IsEnabled], [Email], [Firstname], [Lastname], [Password], [Phone], [User_ID]) VALUES (CAST(N'2020-02-28T02:01:33.0000000' AS DateTime2), 0, N'lectus.ante@yahoo.net', N'Baxter', N'Holland', N'AQAAAAEAACcQAAAAEDiDMNsEk8orl8j2ZIlL0DF6FZkEbXPIfenbAPDYY9J61b/oBRNbA8v1pGEVN8CnQA==', N'+420371507648', 11)
GO
INSERT [dbo].[Users] ([Created], [IsEnabled], [Email], [Firstname], [Lastname], [Password], [Phone], [User_ID]) VALUES (CAST(N'2018-08-10T02:17:38.0000000' AS DateTime2), 0, N'nisi.magna@aol.com', N'Matthew', N'Poole', N'AQAAAAEAACcQAAAAEDiDMNsEk8orl8j2ZIlL0DF6FZkEbXPIfenbAPDYY9J61b/oBRNbA8v1pGEVN8CnQA==', N'+420948741342', 12)
GO
INSERT [dbo].[Users] ([Created], [IsEnabled], [Email], [Firstname], [Lastname], [Password], [Phone], [User_ID]) VALUES (CAST(N'2018-02-14T11:35:08.0000000' AS DateTime2), 1, N'nisi.dictum@yahoo.com', N'Walker', N'Trujillo', N'AQAAAAEAACcQAAAAEDiDMNsEk8orl8j2ZIlL0DF6FZkEbXPIfenbAPDYY9J61b/oBRNbA8v1pGEVN8CnQA==', N'+420427714252', 13)
GO
INSERT [dbo].[Users] ([Created], [IsEnabled], [Email], [Firstname], [Lastname], [Password], [Phone], [User_ID]) VALUES (CAST(N'2020-12-14T04:00:37.0000000' AS DateTime2), 0, N'orci.lacus@protonmail.ca', N'Chester', N'Meyer', N'AQAAAAEAACcQAAAAEDiDMNsEk8orl8j2ZIlL0DF6FZkEbXPIfenbAPDYY9J61b/oBRNbA8v1pGEVN8CnQA==', N'+420320302867', 14)
GO
INSERT [dbo].[Users] ([Created], [IsEnabled], [Email], [Firstname], [Lastname], [Password], [Phone], [User_ID]) VALUES (CAST(N'2020-08-21T10:19:58.0000000' AS DateTime2), 1, N'nibh@protonmail.net', N'Scarlet', N'Franks', N'AQAAAAEAACcQAAAAEDiDMNsEk8orl8j2ZIlL0DF6FZkEbXPIfenbAPDYY9J61b/oBRNbA8v1pGEVN8CnQA==', N'+420778075715', 15)
GO
INSERT [dbo].[Users] ([Created], [IsEnabled], [Email], [Firstname], [Lastname], [Password], [Phone], [User_ID]) VALUES (CAST(N'2019-07-16T10:00:04.0000000' AS DateTime2), 1, N'quam@hotmail.org', N'Sydnee', N'Bond', N'AQAAAAEAACcQAAAAEDiDMNsEk8orl8j2ZIlL0DF6FZkEbXPIfenbAPDYY9J61b/oBRNbA8v1pGEVN8CnQA==', N'+420776079304', 16)
GO
INSERT [dbo].[Users] ([Created], [IsEnabled], [Email], [Firstname], [Lastname], [Password], [Phone], [User_ID]) VALUES (CAST(N'2019-03-13T12:40:08.0000000' AS DateTime2), 1, N'non.vestibulum@aol.net', N'Cheyenne', N'Martin', N'AQAAAAEAACcQAAAAEDiDMNsEk8orl8j2ZIlL0DF6FZkEbXPIfenbAPDYY9J61b/oBRNbA8v1pGEVN8CnQA==', N'+420972320968', 17)
GO
INSERT [dbo].[Users] ([Created], [IsEnabled], [Email], [Firstname], [Lastname], [Password], [Phone], [User_ID]) VALUES (CAST(N'2020-11-27T03:29:11.0000000' AS DateTime2), 0, N'eros.nec.tellus@hotmail.couk', N'Karen', N'Puckett', N'AQAAAAEAACcQAAAAEDiDMNsEk8orl8j2ZIlL0DF6FZkEbXPIfenbAPDYY9J61b/oBRNbA8v1pGEVN8CnQA==', N'+420184736622', 18)
GO
INSERT [dbo].[Users] ([Created], [IsEnabled], [Email], [Firstname], [Lastname], [Password], [Phone], [User_ID]) VALUES (CAST(N'2020-10-24T07:21:06.0000000' AS DateTime2), 0, N'malesuada.ut@protonmail.com', N'Dominique', N'Wilkinson', N'AQAAAAEAACcQAAAAEDiDMNsEk8orl8j2ZIlL0DF6FZkEbXPIfenbAPDYY9J61b/oBRNbA8v1pGEVN8CnQA==', N'+420881678243', 19)
GO
INSERT [dbo].[Users] ([Created], [IsEnabled], [Email], [Firstname], [Lastname], [Password], [Phone], [User_ID]) VALUES (CAST(N'2018-09-07T02:31:37.0000000' AS DateTime2), 1, N'a.odio@icloud.org', N'Wyatt', N'Carey', N'AQAAAAEAACcQAAAAEDiDMNsEk8orl8j2ZIlL0DF6FZkEbXPIfenbAPDYY9J61b/oBRNbA8v1pGEVN8CnQA==', N'+420167634257', 20)
GO
INSERT [dbo].[Users] ([Created], [IsEnabled], [Email], [Firstname], [Lastname], [Password], [Phone], [User_ID]) VALUES (CAST(N'2022-03-20T00:00:00.0000000' AS DateTime2), 1, N'admin@admin.com', N'Admin', N'Admin', N'AQAAAAEAACcQAAAAEOVp3K279lWLYv2FKUjJKUSDs21tGadvpG6k2xHx3axeBEq09iqvw2uDYyNRHr4+Xg==', N'+420123456789', 21)
GO
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
INSERT [dbo].[UsersGroups] ([Group_ID], [User_ID]) VALUES (1, 1)
GO
INSERT [dbo].[UsersGroups] ([Group_ID], [User_ID]) VALUES (1, 2)
GO
INSERT [dbo].[UsersGroups] ([Group_ID], [User_ID]) VALUES (2, 3)
GO
INSERT [dbo].[UsersGroups] ([Group_ID], [User_ID]) VALUES (2, 4)
GO
INSERT [dbo].[UsersGroups] ([Group_ID], [User_ID]) VALUES (3, 5)
GO
INSERT [dbo].[UsersGroups] ([Group_ID], [User_ID]) VALUES (3, 6)
GO
INSERT [dbo].[UsersGroups] ([Group_ID], [User_ID]) VALUES (4, 7)
GO
INSERT [dbo].[UsersGroups] ([Group_ID], [User_ID]) VALUES (4, 8)
GO
INSERT [dbo].[UsersGroups] ([Group_ID], [User_ID]) VALUES (5, 9)
GO
INSERT [dbo].[UsersGroups] ([Group_ID], [User_ID]) VALUES (5, 10)
GO
INSERT [dbo].[UsersGroups] ([Group_ID], [User_ID]) VALUES (1, 21)
GO
INSERT [dbo].[UsersGroups] ([Group_ID], [User_ID]) VALUES (2, 21)
GO
INSERT [dbo].[UsersGroups] ([Group_ID], [User_ID]) VALUES (3, 21)
GO
INSERT [dbo].[UsersGroups] ([Group_ID], [User_ID]) VALUES (4, 21)
GO
INSERT [dbo].[UsersGroups] ([Group_ID], [User_ID]) VALUES (5, 21)
GO
INSERT [dbo].[UsersGroups] ([Group_ID], [User_ID]) VALUES (6, 21)
GO
/****** Object:  Index [IXFK_CustomPropertyOptions_CustomProperties]    Script Date: 20.03.2022 12:16:00 ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[CustomPropertyOptions]') AND name = N'IXFK_CustomPropertyOptions_CustomProperties')
CREATE NONCLUSTERED INDEX [IXFK_CustomPropertyOptions_CustomProperties] ON [dbo].[CustomPropertyOptions]
(
	[CustomProperty_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IXFK_Tickets_Assigned user]    Script Date: 20.03.2022 12:16:00 ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Tickets]') AND name = N'IXFK_Tickets_Assigned user')
CREATE NONCLUSTERED INDEX [IXFK_Tickets_Assigned user] ON [dbo].[Tickets]
(
	[AssignedUser_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Attachements_Comments]') AND parent_object_id = OBJECT_ID(N'[dbo].[Attachements]'))
ALTER TABLE [dbo].[Attachements]  WITH CHECK ADD  CONSTRAINT [FK_Attachements_Comments] FOREIGN KEY([Comment_ID])
REFERENCES [dbo].[Comments] ([Comment_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Attachements_Comments]') AND parent_object_id = OBJECT_ID(N'[dbo].[Attachements]'))
ALTER TABLE [dbo].[Attachements] CHECK CONSTRAINT [FK_Attachements_Comments]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Attachements_Creator]') AND parent_object_id = OBJECT_ID(N'[dbo].[Attachements]'))
ALTER TABLE [dbo].[Attachements]  WITH CHECK ADD  CONSTRAINT [FK_Attachements_Creator] FOREIGN KEY([User_ID])
REFERENCES [dbo].[Users] ([User_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Attachements_Creator]') AND parent_object_id = OBJECT_ID(N'[dbo].[Attachements]'))
ALTER TABLE [dbo].[Attachements] CHECK CONSTRAINT [FK_Attachements_Creator]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Attachements_Tickets]') AND parent_object_id = OBJECT_ID(N'[dbo].[Attachements]'))
ALTER TABLE [dbo].[Attachements]  WITH CHECK ADD  CONSTRAINT [FK_Attachements_Tickets] FOREIGN KEY([Ticket_ID])
REFERENCES [dbo].[Tickets] ([Ticket_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Attachements_Tickets]') AND parent_object_id = OBJECT_ID(N'[dbo].[Attachements]'))
ALTER TABLE [dbo].[Attachements] CHECK CONSTRAINT [FK_Attachements_Tickets]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Comments_Author]') AND parent_object_id = OBJECT_ID(N'[dbo].[Comments]'))
ALTER TABLE [dbo].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_Comments_Author] FOREIGN KEY([User_ID])
REFERENCES [dbo].[Users] ([User_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Comments_Author]') AND parent_object_id = OBJECT_ID(N'[dbo].[Comments]'))
ALTER TABLE [dbo].[Comments] CHECK CONSTRAINT [FK_Comments_Author]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Comments_Tickets]') AND parent_object_id = OBJECT_ID(N'[dbo].[Comments]'))
ALTER TABLE [dbo].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_Comments_Tickets] FOREIGN KEY([Ticket_ID])
REFERENCES [dbo].[Tickets] ([Ticket_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Comments_Tickets]') AND parent_object_id = OBJECT_ID(N'[dbo].[Comments]'))
ALTER TABLE [dbo].[Comments] CHECK CONSTRAINT [FK_Comments_Tickets]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CustomPropertyOptions_CustomProperties]') AND parent_object_id = OBJECT_ID(N'[dbo].[CustomPropertyOptions]'))
ALTER TABLE [dbo].[CustomPropertyOptions]  WITH CHECK ADD  CONSTRAINT [FK_CustomPropertyOptions_CustomProperties] FOREIGN KEY([CustomProperty_ID])
REFERENCES [dbo].[CustomProperties] ([CustomProperty_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CustomPropertyOptions_CustomProperties]') AND parent_object_id = OBJECT_ID(N'[dbo].[CustomPropertyOptions]'))
ALTER TABLE [dbo].[CustomPropertyOptions] CHECK CONSTRAINT [FK_CustomPropertyOptions_CustomProperties]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CustomPropertyValues_CustomPropertyOptions]') AND parent_object_id = OBJECT_ID(N'[dbo].[CustomPropertyValues]'))
ALTER TABLE [dbo].[CustomPropertyValues]  WITH CHECK ADD  CONSTRAINT [FK_CustomPropertyValues_CustomPropertyOptions] FOREIGN KEY([CustomPropertyOption_ID])
REFERENCES [dbo].[CustomPropertyOptions] ([CustomPropertyOption_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CustomPropertyValues_CustomPropertyOptions]') AND parent_object_id = OBJECT_ID(N'[dbo].[CustomPropertyValues]'))
ALTER TABLE [dbo].[CustomPropertyValues] CHECK CONSTRAINT [FK_CustomPropertyValues_CustomPropertyOptions]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CustomPropertyValues_Tickets]') AND parent_object_id = OBJECT_ID(N'[dbo].[CustomPropertyValues]'))
ALTER TABLE [dbo].[CustomPropertyValues]  WITH CHECK ADD  CONSTRAINT [FK_CustomPropertyValues_Tickets] FOREIGN KEY([Ticket_ID])
REFERENCES [dbo].[Tickets] ([Ticket_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CustomPropertyValues_Tickets]') AND parent_object_id = OBJECT_ID(N'[dbo].[CustomPropertyValues]'))
ALTER TABLE [dbo].[CustomPropertyValues] CHECK CONSTRAINT [FK_CustomPropertyValues_Tickets]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Followers_Tickets]') AND parent_object_id = OBJECT_ID(N'[dbo].[Followers]'))
ALTER TABLE [dbo].[Followers]  WITH CHECK ADD  CONSTRAINT [FK_Followers_Tickets] FOREIGN KEY([Ticket_ID])
REFERENCES [dbo].[Tickets] ([Ticket_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Followers_Tickets]') AND parent_object_id = OBJECT_ID(N'[dbo].[Followers]'))
ALTER TABLE [dbo].[Followers] CHECK CONSTRAINT [FK_Followers_Tickets]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Followers_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[Followers]'))
ALTER TABLE [dbo].[Followers]  WITH CHECK ADD  CONSTRAINT [FK_Followers_Users] FOREIGN KEY([User_ID])
REFERENCES [dbo].[Users] ([User_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Followers_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[Followers]'))
ALTER TABLE [dbo].[Followers] CHECK CONSTRAINT [FK_Followers_Users]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ProjectsAccess_Groups]') AND parent_object_id = OBJECT_ID(N'[dbo].[ProjectsAccess]'))
ALTER TABLE [dbo].[ProjectsAccess]  WITH CHECK ADD  CONSTRAINT [FK_ProjectsAccess_Groups] FOREIGN KEY([Group_ID])
REFERENCES [dbo].[Groups] ([Group_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ProjectsAccess_Groups]') AND parent_object_id = OBJECT_ID(N'[dbo].[ProjectsAccess]'))
ALTER TABLE [dbo].[ProjectsAccess] CHECK CONSTRAINT [FK_ProjectsAccess_Groups]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ProjectsAccess_Projects]') AND parent_object_id = OBJECT_ID(N'[dbo].[ProjectsAccess]'))
ALTER TABLE [dbo].[ProjectsAccess]  WITH CHECK ADD  CONSTRAINT [FK_ProjectsAccess_Projects] FOREIGN KEY([Project_ID])
REFERENCES [dbo].[Projects] ([Project_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ProjectsAccess_Projects]') AND parent_object_id = OBJECT_ID(N'[dbo].[ProjectsAccess]'))
ALTER TABLE [dbo].[ProjectsAccess] CHECK CONSTRAINT [FK_ProjectsAccess_Projects]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ProjectsCategories_Categories]') AND parent_object_id = OBJECT_ID(N'[dbo].[ProjectsCategories]'))
ALTER TABLE [dbo].[ProjectsCategories]  WITH CHECK ADD  CONSTRAINT [FK_ProjectsCategories_Categories] FOREIGN KEY([Category_ID])
REFERENCES [dbo].[Categories] ([Category_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ProjectsCategories_Categories]') AND parent_object_id = OBJECT_ID(N'[dbo].[ProjectsCategories]'))
ALTER TABLE [dbo].[ProjectsCategories] CHECK CONSTRAINT [FK_ProjectsCategories_Categories]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ProjectsCategories_Projects]') AND parent_object_id = OBJECT_ID(N'[dbo].[ProjectsCategories]'))
ALTER TABLE [dbo].[ProjectsCategories]  WITH CHECK ADD  CONSTRAINT [FK_ProjectsCategories_Projects] FOREIGN KEY([Project_ID])
REFERENCES [dbo].[Projects] ([Project_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ProjectsCategories_Projects]') AND parent_object_id = OBJECT_ID(N'[dbo].[ProjectsCategories]'))
ALTER TABLE [dbo].[ProjectsCategories] CHECK CONSTRAINT [FK_ProjectsCategories_Projects]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Tickets_Assigned user]') AND parent_object_id = OBJECT_ID(N'[dbo].[Tickets]'))
ALTER TABLE [dbo].[Tickets]  WITH CHECK ADD  CONSTRAINT [FK_Tickets_Assigned user] FOREIGN KEY([AssignedUser_ID])
REFERENCES [dbo].[Users] ([User_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Tickets_Assigned user]') AND parent_object_id = OBJECT_ID(N'[dbo].[Tickets]'))
ALTER TABLE [dbo].[Tickets] CHECK CONSTRAINT [FK_Tickets_Assigned user]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Tickets_Creator]') AND parent_object_id = OBJECT_ID(N'[dbo].[Tickets]'))
ALTER TABLE [dbo].[Tickets]  WITH CHECK ADD  CONSTRAINT [FK_Tickets_Creator] FOREIGN KEY([Creator_ID])
REFERENCES [dbo].[Users] ([User_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Tickets_Creator]') AND parent_object_id = OBJECT_ID(N'[dbo].[Tickets]'))
ALTER TABLE [dbo].[Tickets] CHECK CONSTRAINT [FK_Tickets_Creator]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Tickets_Projects]') AND parent_object_id = OBJECT_ID(N'[dbo].[Tickets]'))
ALTER TABLE [dbo].[Tickets]  WITH CHECK ADD  CONSTRAINT [FK_Tickets_Projects] FOREIGN KEY([Project_ID])
REFERENCES [dbo].[Projects] ([Project_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Tickets_Projects]') AND parent_object_id = OBJECT_ID(N'[dbo].[Tickets]'))
ALTER TABLE [dbo].[Tickets] CHECK CONSTRAINT [FK_Tickets_Projects]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UsersGroups_Groups]') AND parent_object_id = OBJECT_ID(N'[dbo].[UsersGroups]'))
ALTER TABLE [dbo].[UsersGroups]  WITH CHECK ADD  CONSTRAINT [FK_UsersGroups_Groups] FOREIGN KEY([Group_ID])
REFERENCES [dbo].[Groups] ([Group_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UsersGroups_Groups]') AND parent_object_id = OBJECT_ID(N'[dbo].[UsersGroups]'))
ALTER TABLE [dbo].[UsersGroups] CHECK CONSTRAINT [FK_UsersGroups_Groups]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UsersGroups_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[UsersGroups]'))
ALTER TABLE [dbo].[UsersGroups]  WITH CHECK ADD  CONSTRAINT [FK_UsersGroups_Users] FOREIGN KEY([User_ID])
REFERENCES [dbo].[Users] ([User_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UsersGroups_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[UsersGroups]'))
ALTER TABLE [dbo].[UsersGroups] CHECK CONSTRAINT [FK_UsersGroups_Users]
GO
IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Attachements', N'COLUMN',N'Uploaded'))
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'a' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Attachements', @level2type=N'COLUMN',@level2name=N'Uploaded'
GO

CREATE OR ALTER FUNCTION fn_active_followers (@TicketId INT)
RETURNS INT
AS
BEGIN
	DECLARE @Count INT;
	
	SELECT 
		@Count = COUNT(*) 
	FROM Followers fl
	INNER JOIN Users u ON fl.User_ID = u.User_ID
	WHERE 
		fl.Ticket_ID = @TicketId AND 
		u.IsEnabled = 1;

	RETURN (@Count);
END;
GO

CREATE OR ALTER FUNCTION fn_user_fullName (@UserId INT)
RETURNS NVARCHAR(101)
AS
BEGIN
	DECLARE @FullName NVARCHAR(101);
	
	SELECT 
		@FullName = u.Firstname + ' ' + u.Lastname 
	FROM Users u
	WHERE u.User_ID = @UserId;

	RETURN (@FullName);
END;
GO

CREATE OR ALTER FUNCTION fn_user_type (@UserId INT)
RETURNS INT
AS 
BEGIN
	DECLARE @Type INT;

	SELECT 
		@Type = SUM(sub.Type)
	FROM (
		SELECT DISTINCT
			g.Type
		FROM Groups g
		INNER JOIN UsersGroups ug ON ug.Group_ID = g.Group_ID
		WHERE
			ug.User_ID = @UserId
	) sub;

	RETURN (@Type);
END;
GO


CREATE OR ALTER PROCEDURE proc_anonymize_user @UserId INT
AS
	UPDATE Users
	SET
		Email = 'no-reply@fixatic.eu',
		Firstname = 'Deleted',
		Lastname = 'User',
		IsEnabled = 0,
		Phone = ''
	WHERE User_ID = @UserId
GO

CREATE OR ALTER PROCEDURE proc_finish_ticket @TicketId INT
AS
	UPDATE Tickets
	SET
		DateSolved = SYSDATETIME(),
		Status = 3,
		AssignedUser_ID = NULL
	WHERE Ticket_ID = @TicketId
GO

CREATE OR ALTER PROCEDURE proc_create_user (@email NVARCHAR(100), @fname NVARCHAR(50), @lname NVARCHAR(50), @pwdHash NVARCHAR(250), @phone NVARCHAR(50), @NewID INT OUTPUT)
AS
	SET NOCOUNT ON;
	
	INSERT INTO Users (Created, IsEnabled, Email, Firstname, Lastname, Password, Phone) 
	VALUES (SYSDATETIME(), 1, @email, @fname, @lname, @pwdHash, @phone);

	SET @NewID = SCOPE_IDENTITY();

	RETURN;
GO

CREATE OR ALTER TRIGGER Projects_delete_trigger
ON Projects
INSTEAD OF DELETE
AS
	DECLARE @ProjectId INT

	SELECT @ProjectId = deleted.Project_ID FROM deleted;

	IF (EXISTS(SELECT 1 FROM Tickets t WHERE t.Project_ID = @ProjectId))
	BEGIN
		RAISERROR('Project cannot be deleted while is referenced by Ticket', 16,1);
		ROLLBACK TRANSACTION;
        RETURN;
	END

	DELETE FROM ProjectsAccess WHERE Project_ID = @ProjectId;
	DELETE FROM ProjectsCategories WHERE Project_ID = @ProjectId;

GO

CREATE OR ALTER TRIGGER Users_delete_trigger
ON Users
INSTEAD OF DELETE
AS
	DECLARE @UserId INT

	SELECT @UserId = deleted.User_ID FROM deleted;

	EXECUTE proc_anonymize_user @UserId
GO

CREATE OR ALTER VIEW view_tickets
AS
	SELECT
		Ticket_ID, 
		Title, 
		Content, 
		Created, 
		Modified, 
		DateSolved, 
		Priority, 
		Status, 
		Type, 
		Visibility, 
		Project_ID, 
		AssignedUser_ID, 
		Creator_ID,
		Followers = dbo.fn_active_followers(Ticket_ID),
		AssigneeName = dbo.fn_user_fullName(AssignedUser_ID)
	FROM Tickets
GO

CREATE OR ALTER VIEW view_properties
AS 
	SELECT
		cp.Name,
		cp.Description,
		co.CustomPropertyOption_ID, 
		co.Content,
		co.IsEnabled, 
		co.Sequence, 
		co.CustomProperty_ID,
		cv.Ticket_ID
	FROM CustomPropertyOptions co
	INNER JOIN CustomPropertyValues cv ON co.CustomPropertyOption_ID = cv.CustomPropertyOption_ID
	INNER JOIN CustomProperties cp ON cp.CustomProperty_ID = co.CustomProperty_ID
GO

CREATE OR ALTER VIEW view_full_users
AS 
	SELECT
		u.User_ID,
		u.Firstname,
		u.Lastname,
		u.Email,
		u.Phone,
		u.Created,
		u.IsEnabled,
		Type = dbo.fn_user_type(u.User_ID)
	FROM Users u
GO