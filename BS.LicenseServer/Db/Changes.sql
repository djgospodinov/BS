ALTER TABLE [dbo].[Licenses] DROP CONSTRAINT DF__Licenses__Workst__5BE2A6F2
GO

ALTER TABLE [dbo].[Licenses] DROP COLUMN [WorkstationCount]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LicenseActivation](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LicenseId] [uniqueidentifier] NOT NULL,
	[ComputerId] [nvarchar](256) NULL,
	[UserId] [nvarchar](256) NULL,
	[ComputerCount] [int] NULL,
 CONSTRAINT [PK_LicenseActivation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[LicenseActivation]  WITH CHECK ADD  CONSTRAINT [FK_LicenseActivation_Licenses] FOREIGN KEY([LicenseId])
REFERENCES [dbo].[Licenses] ([Id])
GO

ALTER TABLE [dbo].[LicenseActivation] CHECK CONSTRAINT [FK_LicenseActivation_Licenses]
GO

--------------------------------------------8/112017
CREATE TABLE [dbo].[IpFilters](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Address] [nchar](50) NOT NULL,
	[Denied] [bit] NOT NULL,
 CONSTRAINT [PK_IpRestrictions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Settings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UseIPFilter] [bit] NOT NULL,
 CONSTRAINT [PK_Settings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE LicenseOwners ADD EGN nchar(10) NULL
GO

----------------------------11.15.2017
 ALTER TABLE [dbo].[lu_LicenseModules] ADD Code NVARCHAR(20) NULL;
  GO

  UPDATE [dbo].[lu_LicenseModules]
  SET Code = 'Accounting'
  WHERE Id = 1

  UPDATE [dbo].[lu_LicenseModules]
  SET Code = 'Production'
  WHERE Id = 2

  UPDATE [dbo].[lu_LicenseModules]
  SET Code = 'Warehouse'
  WHERE Id = 3

  UPDATE [dbo].[lu_LicenseModules]
  SET Code = 'TradingSystem'
  WHERE Id = 4

  UPDATE [dbo].[lu_LicenseModules]
  SET Code = 'Salary'
  WHERE Id = 5

  UPDATE [dbo].[lu_LicenseModules]
  SET Code = 'Schedules'
  WHERE Id = 6

  GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LicensesLog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LicenseId] [uniqueidentifier] NOT NULL,
	[Date] [datetime] NOT NULL,
	[IsDemo] [bit] NOT NULL,
	[Changes] [ntext] NOT NULL,
 CONSTRAINT [PK_LicensesLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

-------------------------------11/15/2017
EXEC sp_RENAME 'LicenseOwnerExtraInfo.DDSRegistration' , 'VatRegistration', 'COLUMN'
GO
--------------------------------01/12/2017
ALTER TABLE LicenseModules ADD ValidTo DATETIME NOT NULL DEFAULT GETDATE();
GO
ALTER TABLE LicensesLog ADD ChangeType INT NOT NULL;
GO
ALTER TABLE Licenses ADD WorkstationsCount INT NOT NULL DEFAULT 1;
GO

ALTER TABLE LicenseActivation ADD ComputerName NVARCHAR(256) NULL;
GO

---------------------------------
DELETE FROM [dbo].[lu_LicenseModules]
WHERE ID > 4

DELETE FROM [dbo].[LicenseModules]
WHERE ModuleId > 4

UPDATE [lu_LicenseModules]
SET Name = 'ТРЗ и личен състав',
	Code = 'Payroll'
WHERE Id = 2

UPDATE [lu_LicenseModules]
SET Name = 'Складов софтуер',
	Code = 'Store'
WHERE Id = 3

UPDATE [lu_LicenseModules]
SET Name = 'Графици на работа',
	Code = 'Schedule'
WHERE Id = 4

----------------------------------------28/01/2018
  ALTER TABLE [dbo].[LicensesLog] ADD ChangedBy BIGINT NOT NULL DEFAULT 0;
  --------------------------------------31/01/2018

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP TABLE [dbo].[LicensesLog]
GO

CREATE TABLE [dbo].[LicensesLog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LicenseId] [uniqueidentifier] NOT NULL,
	[Date] [datetime] NOT NULL,
	[IsDemo] [bit] NOT NULL,
	[Old] [ntext] NULL,
	[New] [ntext] NOT NULL,
	[ChangedBy] [bigint] NOT NULL,
 CONSTRAINT [PK_LicensesLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[LicensesLog] ADD  DEFAULT ((0)) FOR [ChangedBy]
GO

-------------------------------------------------------------------31/01/2018
DROP TABLE [dbo].[ApiLogs]
GO

CREATE TABLE [dbo].[ApiLogs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AbsoluteUri] [nvarchar](250) NOT NULL,
	[Host] [nvarchar](250) NOT NULL,
	[RequestUri] [nvarchar](250) NOT NULL,
	[RequestMethod] [nvarchar](10) NOT NULL,
	[RequestBody] [nvarchar](max) NULL,
	[RequestIpAddress] [nvarchar](50) NOT NULL,
	[RequestTimestamp] [datetime] NOT NULL,
	[ResponseContentBody] [nvarchar](max) NULL,
	[ResponseStatusCode] [int] NULL,
	[ResponseTimestamp] [datetime] NULL,
 CONSTRAINT [PK_ApiLogs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

---------------------------------------------25/2/2018------------------------------------------
DROP TABLE [dbo].[LicenseVariables]
GO

DROP TABLE [dbo].[lu_LicenseVariables]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[lu_LicenseVariables](
	[Id] [int] IDENTITY NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_lu_LicenseVariables] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[LicenseVariables](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LicenseId] [uniqueidentifier] NOT NULL,
	[VariableId] [int] NOT NULL,
	[Value] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_LicenseVariables] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[LicenseVariables]  WITH CHECK ADD  CONSTRAINT [FK_Licenses_LicenseVariables] FOREIGN KEY([LicenseId])
REFERENCES [dbo].[Licenses] ([Id])
GO

ALTER TABLE [dbo].[LicenseVariables] CHECK CONSTRAINT [FK_Licenses_LicenseVariables]
GO

ALTER TABLE [dbo].[LicenseVariables]  WITH CHECK ADD  CONSTRAINT [FK_lu_LicenseVariables_LicenseVariables] FOREIGN KEY([VariableId])
REFERENCES [dbo].[lu_LicenseVariables] ([Id])
GO

ALTER TABLE [dbo].[LicenseVariables] CHECK CONSTRAINT [FK_lu_LicenseVariables_LicenseVariables]
GO
-----------------------------------27/02/2018-------------------------------------------------------
ALTER TABLE [dbo].[Licenses] ADD ExternalId NVARCHAR(50) NULL;
GO
--------------------------------------From Petko----------------------------------------------------

IF OBJECT_ID(N'[dbo].[LicenseOwnerServer]', 'U') IS NOT NULL
DROP TABLE [dbo].[LicenseOwnerServer]
GO

CREATE TABLE LicenseOwnerServer
( 
	LicenseOwnerID       int  NOT NULL ,
	ServerName           varchar(255)  NOT NULL ,
	CreateDate           datetime  NOT NULL 
)
go

IF NOT EXISTS (
  SELECT * 
  FROM   sys.columns 
  WHERE  object_id = OBJECT_ID(N'[dbo].[LicenseActivation]') 
         AND name = 'DiskNumCr'
)
ALTER TABLE LicenseActivation ADD DiskNumCr varchar(255)
GO
IF NOT EXISTS (
  SELECT * 
  FROM   sys.columns 
  WHERE  object_id = OBJECT_ID(N'[dbo].[LicenseActivation]') 
         AND name = 'RegUserNameCr'
)
ALTER TABLE LicenseActivation ADD RegUserNameCr varchar(255)
GO
IF NOT EXISTS (
  SELECT * 
  FROM   sys.columns 
  WHERE  object_id = OBJECT_ID(N'[dbo].[LicenseOwnerExtraInfo]') 
         AND name = 'IsVIP'
)
ALTER TABLE LicenseOwnerExtraInfo ADD IsVIP bit NOT NULL DEFAULT 0
GO
IF NOT EXISTS (
  SELECT * 
  FROM   sys.columns 
  WHERE  object_id = OBJECT_ID(N'[dbo].[LicenseOwnerExtraInfo]') 
         AND name = 'VIPComment'
)
ALTER TABLE LicenseOwnerExtraInfo ADD VIPComment varchar(255)
GO
IF NOT EXISTS (
  SELECT * 
  FROM   sys.columns 
  WHERE  object_id = OBJECT_ID(N'[dbo].[lu_LicenseModules]') 
         AND name = 'MasterID'
)
ALTER TABLE lu_LicenseModules ADD MasterID int
GO
IF NOT EXISTS (
  SELECT * 
  FROM   sys.columns 
  WHERE  object_id = OBJECT_ID(N'[dbo].[lu_LicenseModules]') 
         AND name = 'bActive'
)
ALTER TABLE lu_LicenseModules ADD bActive bit
GO
IF NOT EXISTS (
  SELECT * 
  FROM   sys.columns 
  WHERE  object_id = OBJECT_ID(N'[dbo].[lu_LicenseModules]') 
         AND name = 'OrderNo'
)
ALTER TABLE lu_LicenseModules ADD OrderNo int
GO

ALTER TABLE LicenseOwnerServer
	ADD CONSTRAINT XPKLicenseOwnersServer PRIMARY KEY  CLUSTERED (LicenseOwnerID ASC)
go

IF NOT EXISTS (
  SELECT * 
  FROM   sys.columns 
  WHERE  object_id = OBJECT_ID(N'[dbo].[LicenseActivation]') 
         AND name = 'LicenseOwnerServerId'
)
ALTER TABLE LicenseActivation ADD LicenseOwnerServerId int
GO
-------------------------Remove not needed tables
ALTER TABLE LicenseOwners DROP FK_LicenseOwners_AspNetUsers
GO

IF EXISTS(SELECT OBJECT_NAME(OBJECT_ID) AS NameofConstraint
    FROM sys.objects
    WHERE type_desc LIKE '%CONSTRAINT'
        AND OBJECT_NAME(OBJECT_ID)='DF__LicenseOw__UserI__45F365D3')
BEGIN
	ALTER TABLE LicenseOwners DROP DF__LicenseOw__UserI__45F365D3
END
GO

ALTER TABLE LicenseOwners DROP COLUMN UserId
GO

DROP TABLE AspNetUserRoles
GO

DROP TABLE AspNetRoles
GO

DROP TABLE AspNetUserClaims
GO

DROP TABLE AspNetUserLogins
GO

DROP TABLE AspNetUsers
GO