
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 10/10/2017 22:49:09
-- Generated from EDMX file: c:\Projects\BS.Api\BS.LicenseServer\Db\LicenseDb.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [LicenseDb];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_LicenseModulesLicenses]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LicenseModules] DROP CONSTRAINT [FK_LicenseModulesLicenses];
GO
IF OBJECT_ID(N'[dbo].[FK_LicenseOwnerExtraInfo_LicenseOwners]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LicenseOwnerExtraInfo] DROP CONSTRAINT [FK_LicenseOwnerExtraInfo_LicenseOwners];
GO
IF OBJECT_ID(N'[dbo].[FK_LicensesLicenseOwners]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Licenses] DROP CONSTRAINT [FK_LicensesLicenseOwners];
GO
IF OBJECT_ID(N'[dbo].[FK_lu_LicenseModulesLicenseModules]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LicenseModules] DROP CONSTRAINT [FK_lu_LicenseModulesLicenseModules];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[LicenseModules]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LicenseModules];
GO
IF OBJECT_ID(N'[dbo].[LicenseOwnerExtraInfo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LicenseOwnerExtraInfo];
GO
IF OBJECT_ID(N'[dbo].[LicenseOwners]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LicenseOwners];
GO
IF OBJECT_ID(N'[dbo].[Licenses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Licenses];
GO
IF OBJECT_ID(N'[dbo].[lu_LicenseModules]', 'U') IS NOT NULL
    DROP TABLE [dbo].[lu_LicenseModules];
GO
IF OBJECT_ID(N'[LicenseDbModelStoreContainer].[lu_LicenseTypes]', 'U') IS NOT NULL
    DROP TABLE [LicenseDbModelStoreContainer].[lu_LicenseTypes];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'LicenseModules'
CREATE TABLE [dbo].[LicenseModules] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [LicensesId] uniqueidentifier  NOT NULL,
    [ModuleId] smallint  NOT NULL
);
GO

-- Creating table 'LicenseOwners'
CREATE TABLE [dbo].[LicenseOwners] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Email] nvarchar(max)  NOT NULL,
    [Phone] nvarchar(max)  NOT NULL,
    [IsCompany] bit  NOT NULL,
    [ContactPerson] nvarchar(max)  NOT NULL,
    [CompanyId] nchar(15)  NULL
);
GO

-- Creating table 'Licenses'
CREATE TABLE [dbo].[Licenses] (
    [Id] uniqueidentifier  NOT NULL,
    [ValidTo] datetime  NOT NULL,
    [IsDemo] bit  NOT NULL,
    [LicenseOwnerId] int  NOT NULL,
    [Enabled] bit  NULL,
    [WorkstationCount] int  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [SubscribedTo] datetime  NULL,
    [Type] tinyint  NULL
);
GO

-- Creating table 'lu_LicenseModules'
CREATE TABLE [dbo].[lu_LicenseModules] (
    [Id] smallint IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'LicenseOwnerExtraInfoes'
CREATE TABLE [dbo].[LicenseOwnerExtraInfoes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [LicenseOwnerId] int  NOT NULL,
    [PostCode] int  NULL,
    [RegistrationAddress] nchar(250)  NULL,
    [PostAddress] nchar(250)  NULL,
    [MOL] nvarchar(250)  NULL,
    [ContactPerson] nvarchar(250)  NULL,
    [AccountingPerson] nvarchar(250)  NULL,
    [DDSRegistration] bit  NULL
);
GO

-- Creating table 'lu_LicenseTypes'
CREATE TABLE [dbo].[lu_LicenseTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nchar(20)  NOT NULL,
    [IsOnline] bit  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'LicenseModules'
ALTER TABLE [dbo].[LicenseModules]
ADD CONSTRAINT [PK_LicenseModules]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'LicenseOwners'
ALTER TABLE [dbo].[LicenseOwners]
ADD CONSTRAINT [PK_LicenseOwners]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Licenses'
ALTER TABLE [dbo].[Licenses]
ADD CONSTRAINT [PK_Licenses]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'lu_LicenseModules'
ALTER TABLE [dbo].[lu_LicenseModules]
ADD CONSTRAINT [PK_lu_LicenseModules]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'LicenseOwnerExtraInfoes'
ALTER TABLE [dbo].[LicenseOwnerExtraInfoes]
ADD CONSTRAINT [PK_LicenseOwnerExtraInfoes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id], [Name], [IsOnline] in table 'lu_LicenseTypes'
ALTER TABLE [dbo].[lu_LicenseTypes]
ADD CONSTRAINT [PK_lu_LicenseTypes]
    PRIMARY KEY CLUSTERED ([Id], [Name], [IsOnline] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [LicensesId] in table 'LicenseModules'
ALTER TABLE [dbo].[LicenseModules]
ADD CONSTRAINT [FK_LicenseModulesLicenses]
    FOREIGN KEY ([LicensesId])
    REFERENCES [dbo].[Licenses]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_LicenseModulesLicenses'
CREATE INDEX [IX_FK_LicenseModulesLicenses]
ON [dbo].[LicenseModules]
    ([LicensesId]);
GO

-- Creating foreign key on [ModuleId] in table 'LicenseModules'
ALTER TABLE [dbo].[LicenseModules]
ADD CONSTRAINT [FK_lu_LicenseModulesLicenseModules]
    FOREIGN KEY ([ModuleId])
    REFERENCES [dbo].[lu_LicenseModules]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_lu_LicenseModulesLicenseModules'
CREATE INDEX [IX_FK_lu_LicenseModulesLicenseModules]
ON [dbo].[LicenseModules]
    ([ModuleId]);
GO

-- Creating foreign key on [LicenseOwnerId] in table 'Licenses'
ALTER TABLE [dbo].[Licenses]
ADD CONSTRAINT [FK_LicensesLicenseOwners]
    FOREIGN KEY ([LicenseOwnerId])
    REFERENCES [dbo].[LicenseOwners]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_LicensesLicenseOwners'
CREATE INDEX [IX_FK_LicensesLicenseOwners]
ON [dbo].[Licenses]
    ([LicenseOwnerId]);
GO

-- Creating foreign key on [LicenseOwnerId] in table 'LicenseOwnerExtraInfoes'
ALTER TABLE [dbo].[LicenseOwnerExtraInfoes]
ADD CONSTRAINT [FK_LicenseOwnerExtraInfo_LicenseOwners]
    FOREIGN KEY ([LicenseOwnerId])
    REFERENCES [dbo].[LicenseOwners]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_LicenseOwnerExtraInfo_LicenseOwners'
CREATE INDEX [IX_FK_LicenseOwnerExtraInfo_LicenseOwners]
ON [dbo].[LicenseOwnerExtraInfoes]
    ([LicenseOwnerId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------

INSERT INTO [dbo].[lu_LicenseModules]
           ([Name])
     VALUES
           ('Счетоводство'), ('Производство'), ('Склад'), ('Търговска система'), ('ТРЗ'), ('Графици')

INSERT [dbo].[lu_LicenseTypes] ([Name], [IsOnline]) VALUES (N'Per Computer', 1)
GO
INSERT [dbo].[lu_LicenseTypes] ([Name], [IsOnline]) VALUES (N'Per User', 1)
GO
INSERT [dbo].[lu_LicenseTypes] ([Name], [IsOnline]) VALUES (N'Per Server', 1)