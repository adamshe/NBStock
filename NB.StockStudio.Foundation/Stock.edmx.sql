
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 01/31/2015 16:11:06
-- Generated from EDMX file: E:\Development\TradingProjects\Easy Financial Chart3.5 .net 4.5\StockExpert\NB.StockStudio.Foundation\Stock.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Stock];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[condition]', 'U') IS NOT NULL
    DROP TABLE [dbo].[condition];
GO
IF OBJECT_ID(N'[dbo].[Intraday]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Intraday];
GO
IF OBJECT_ID(N'[dbo].[realtime]', 'U') IS NOT NULL
    DROP TABLE [dbo].[realtime];
GO
IF OBJECT_ID(N'[StockStoreContainer].[stockdata]', 'U') IS NOT NULL
    DROP TABLE [StockStoreContainer].[stockdata];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'conditions'
CREATE TABLE [dbo].[conditions] (
    [ConditionId] int IDENTITY(1,1) NOT NULL,
    [Condition1] varchar(200)  NULL,
    [ConditionDesc] varchar(80)  NULL,
    [Exchange] varchar(10)  NULL,
    [Total] int  NULL,
    [Scaned] int  NULL,
    [ResultCount] int  NULL,
    [StartTime] datetime  NULL,
    [EndTime] datetime  NULL,
    [ScanType] int  NULL
);
GO

-- Creating table 'Intradays'
CREATE TABLE [dbo].[Intradays] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Symbol] varchar(10)  NULL,
    [QuoteTime] datetime  NULL,
    [Price] float  NULL,
    [Volume] float  NULL
);
GO

-- Creating table 'realtimes'
CREATE TABLE [dbo].[realtimes] (
    [QuoteCode] varchar(20)  NOT NULL,
    [LastA] float  NULL,
    [OpenA] float  NULL,
    [HighA] float  NULL,
    [LowA] float  NULL,
    [CloseA] float  NULL,
    [VolumeA] float  NULL,
    [AMountA] float  NULL,
    [LastTime] datetime  NULL
);
GO

-- Creating table 'stockdatas'
CREATE TABLE [dbo].[stockdatas] (
    [Sid] int  NOT NULL,
    [QuoteCode] varchar(20)  NULL,
    [QuoteName] varchar(80)  NULL,
    [Exchange] varchar(20)  NULL,
    [HasHistory] int  NOT NULL,
    [AliasCode] varchar(10)  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ConditionId] in table 'conditions'
ALTER TABLE [dbo].[conditions]
ADD CONSTRAINT [PK_conditions]
    PRIMARY KEY CLUSTERED ([ConditionId] ASC);
GO

-- Creating primary key on [Id] in table 'Intradays'
ALTER TABLE [dbo].[Intradays]
ADD CONSTRAINT [PK_Intradays]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [QuoteCode] in table 'realtimes'
ALTER TABLE [dbo].[realtimes]
ADD CONSTRAINT [PK_realtimes]
    PRIMARY KEY CLUSTERED ([QuoteCode] ASC);
GO

-- Creating primary key on [HasHistory], [Sid] in table 'stockdatas'
ALTER TABLE [dbo].[stockdatas]
ADD CONSTRAINT [PK_stockdatas]
    PRIMARY KEY CLUSTERED ([HasHistory], [Sid] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------