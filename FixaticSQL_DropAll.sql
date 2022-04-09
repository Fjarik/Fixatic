USE [Fixatic]
GO
ALTER TABLE [dbo].[UsersGroups] DROP CONSTRAINT [FK_UsersGroups_Users]
GO
ALTER TABLE [dbo].[UsersGroups] DROP CONSTRAINT [FK_UsersGroups_Groups]
GO
ALTER TABLE [dbo].[Tickets] DROP CONSTRAINT [FK_Tickets_Projects]
GO
ALTER TABLE [dbo].[Tickets] DROP CONSTRAINT [FK_Tickets_Creator]
GO
ALTER TABLE [dbo].[Tickets] DROP CONSTRAINT [FK_Tickets_Assigned user]
GO
ALTER TABLE [dbo].[ProjectsCategories] DROP CONSTRAINT [FK_ProjectsCategories_Projects]
GO
ALTER TABLE [dbo].[ProjectsCategories] DROP CONSTRAINT [FK_ProjectsCategories_Categories]
GO
ALTER TABLE [dbo].[ProjectsAccess] DROP CONSTRAINT [FK_ProjectsAccess_Projects]
GO
ALTER TABLE [dbo].[ProjectsAccess] DROP CONSTRAINT [FK_ProjectsAccess_Groups]
GO
ALTER TABLE [dbo].[Followers] DROP CONSTRAINT [FK_Followers_Users]
GO
ALTER TABLE [dbo].[Followers] DROP CONSTRAINT [FK_Followers_Tickets]
GO
ALTER TABLE [dbo].[CustomPropertyValues] DROP CONSTRAINT [FK_CustomPropertyValues_Tickets]
GO
ALTER TABLE [dbo].[CustomPropertyValues] DROP CONSTRAINT [FK_CustomPropertyValues_CustomPropertyOptions]
GO
ALTER TABLE [dbo].[CustomPropertyOptions] DROP CONSTRAINT [FK_CustomPropertyOptions_CustomProperties]
GO
ALTER TABLE [dbo].[Comments] DROP CONSTRAINT [FK_Comments_Tickets]
GO
ALTER TABLE [dbo].[Comments] DROP CONSTRAINT [FK_Comments_Author]
GO
ALTER TABLE [dbo].[Attachements] DROP CONSTRAINT [FK_Attachements_Tickets]
GO
ALTER TABLE [dbo].[Attachements] DROP CONSTRAINT [FK_Attachements_Creator]
GO
ALTER TABLE [dbo].[Attachements] DROP CONSTRAINT [FK_Attachements_Comments]
GO
/****** Object:  Table [dbo].[UsersGroups]    Script Date: 20.03.2022 12:21:45 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UsersGroups]') AND type in (N'U'))
DROP TABLE [dbo].[UsersGroups]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 20.03.2022 12:21:45 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
DROP TABLE [dbo].[Users]
GO
/****** Object:  Table [dbo].[Tickets]    Script Date: 20.03.2022 12:21:45 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Tickets]') AND type in (N'U'))
DROP TABLE [dbo].[Tickets]
GO
/****** Object:  Table [dbo].[ProjectsCategories]    Script Date: 20.03.2022 12:21:45 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProjectsCategories]') AND type in (N'U'))
DROP TABLE [dbo].[ProjectsCategories]
GO
/****** Object:  Table [dbo].[ProjectsAccess]    Script Date: 20.03.2022 12:21:45 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProjectsAccess]') AND type in (N'U'))
DROP TABLE [dbo].[ProjectsAccess]
GO
/****** Object:  Table [dbo].[Projects]    Script Date: 20.03.2022 12:21:45 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Projects]') AND type in (N'U'))
DROP TABLE [dbo].[Projects]
GO
/****** Object:  Table [dbo].[Groups]    Script Date: 20.03.2022 12:21:45 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Groups]') AND type in (N'U'))
DROP TABLE [dbo].[Groups]
GO
/****** Object:  Table [dbo].[Followers]    Script Date: 20.03.2022 12:21:45 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Followers]') AND type in (N'U'))
DROP TABLE [dbo].[Followers]
GO
/****** Object:  Table [dbo].[CustomPropertyValues]    Script Date: 20.03.2022 12:21:45 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CustomPropertyValues]') AND type in (N'U'))
DROP TABLE [dbo].[CustomPropertyValues]
GO
/****** Object:  Table [dbo].[CustomPropertyOptions]    Script Date: 20.03.2022 12:21:45 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CustomPropertyOptions]') AND type in (N'U'))
DROP TABLE [dbo].[CustomPropertyOptions]
GO
/****** Object:  Table [dbo].[CustomProperties]    Script Date: 20.03.2022 12:21:45 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CustomProperties]') AND type in (N'U'))
DROP TABLE [dbo].[CustomProperties]
GO
/****** Object:  Table [dbo].[Comments]    Script Date: 20.03.2022 12:21:45 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Comments]') AND type in (N'U'))
DROP TABLE [dbo].[Comments]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 20.03.2022 12:21:45 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Categories]') AND type in (N'U'))
DROP TABLE [dbo].[Categories]
GO
/****** Object:  Table [dbo].[Attachements]    Script Date: 20.03.2022 12:21:45 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Attachements]') AND type in (N'U'))
DROP TABLE [dbo].[Attachements]
GO
