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

