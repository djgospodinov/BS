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