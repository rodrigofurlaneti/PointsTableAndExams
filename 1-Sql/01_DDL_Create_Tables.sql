-- ============================================================
-- POINTS TABLE AND EXAMS SYSTEM
-- Database Modeling: SQL Server
-- Standard: US English naming conventions
-- PKs: UNIQUEIDENTIFIER (Guid) — matches .NET Domain model
-- ============================================================

USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'PointsTableAndExams')
BEGIN
    CREATE DATABASE PointsTableAndExams
    COLLATE Latin1_General_CI_AI;
END
GO

USE PointsTableAndExams;
GO

-- Drop in reverse dependency order
IF OBJECT_ID('dbo.ExamRequestItem', 'U') IS NOT NULL DROP TABLE dbo.ExamRequestItem;
IF OBJECT_ID('dbo.ExamRequest',     'U') IS NOT NULL DROP TABLE dbo.ExamRequest;
IF OBJECT_ID('dbo.Exam',            'U') IS NOT NULL DROP TABLE dbo.Exam;
IF OBJECT_ID('dbo.ExamCategory',    'U') IS NOT NULL DROP TABLE dbo.ExamCategory;
IF OBJECT_ID('dbo.DailyLogItem',    'U') IS NOT NULL DROP TABLE dbo.DailyLogItem;
IF OBJECT_ID('dbo.DailyLog',        'U') IS NOT NULL DROP TABLE dbo.DailyLog;
IF OBJECT_ID('dbo.FoodItem',        'U') IS NOT NULL DROP TABLE dbo.FoodItem;
IF OBJECT_ID('dbo.FoodCategory',    'U') IS NOT NULL DROP TABLE dbo.FoodCategory;
IF OBJECT_ID('dbo.Users',           'U') IS NOT NULL DROP TABLE dbo.Users;
GO

-- ============================================================
-- 1. USERS
-- ============================================================
CREATE TABLE dbo.Users (
    Id              UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    FullName        NVARCHAR(150)    NOT NULL,
    Email           NVARCHAR(150)    NOT NULL,
    PhoneNumber     NVARCHAR(20)     NULL,
    BirthDate       DATE             NULL,
    Gender          NVARCHAR(10)     NOT NULL,   -- 'Male' | 'Female' | 'Other'
    Username        NVARCHAR(80)     NOT NULL,
    PasswordHash    NVARCHAR(256)    NOT NULL,
    IsActive        BIT              NOT NULL DEFAULT 1,
    CreatedAt       DATETIME2        NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt       DATETIME2        NULL,

    CONSTRAINT PK_Users             PRIMARY KEY (Id),
    CONSTRAINT UQ_Users_Email       UNIQUE (Email),
    CONSTRAINT UQ_Users_Username    UNIQUE (Username)
);
GO

-- ============================================================
-- 2. FOOD POINTS TABLE
-- ============================================================

CREATE TABLE dbo.FoodCategory (
    Id                  UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    Name                NVARCHAR(100)    NOT NULL,
    Description         NVARCHAR(300)    NULL,
    DefaultQuotaPoints  SMALLINT         NULL,
    ServingUnit         NVARCHAR(100)    NULL,
    SortOrder           TINYINT          NOT NULL DEFAULT 0,
    IsActive            BIT              NOT NULL DEFAULT 1,

    CONSTRAINT PK_FoodCategory PRIMARY KEY (Id)
);
GO

CREATE TABLE dbo.FoodItem (
    Id              UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    FoodCategoryId  UNIQUEIDENTIFIER NOT NULL,
    Name            NVARCHAR(150)    NOT NULL,
    ServingSize     NVARCHAR(100)    NULL,
    Points          SMALLINT         NOT NULL DEFAULT 0,
    Notes           NVARCHAR(300)    NULL,
    IsActive        BIT              NOT NULL DEFAULT 1,

    CONSTRAINT PK_FoodItem              PRIMARY KEY (Id),
    CONSTRAINT FK_FoodItem_Category     FOREIGN KEY (FoodCategoryId)
        REFERENCES dbo.FoodCategory (Id)
);
GO

CREATE TABLE dbo.DailyLog (
    Id          UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    UserId      UNIQUEIDENTIFIER NOT NULL,
    LogDate     DATE             NOT NULL,
    TotalPoints SMALLINT         NOT NULL DEFAULT 0,
    Notes       NVARCHAR(500)    NULL,
    CreatedAt   DATETIME2        NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt   DATETIME2        NULL,

    CONSTRAINT PK_DailyLog              PRIMARY KEY (Id),
    CONSTRAINT FK_DailyLog_User         FOREIGN KEY (UserId)
        REFERENCES dbo.Users (Id),
    CONSTRAINT UQ_DailyLog_UserDate     UNIQUE (UserId, LogDate)
);
GO

CREATE TABLE dbo.DailyLogItem (
    Id              UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    DailyLogId      UNIQUEIDENTIFIER NOT NULL,
    FoodItemId      UNIQUEIDENTIFIER NOT NULL,
    Quantity        DECIMAL(5,2)     NOT NULL DEFAULT 1,
    PointsComputed  SMALLINT         NOT NULL DEFAULT 0,
    MealTime        TIME             NULL,
    Notes           NVARCHAR(200)    NULL,

    CONSTRAINT PK_DailyLogItem              PRIMARY KEY (Id),
    CONSTRAINT FK_DailyLogItem_DailyLog     FOREIGN KEY (DailyLogId)
        REFERENCES dbo.DailyLog (Id),
    CONSTRAINT FK_DailyLogItem_FoodItem     FOREIGN KEY (FoodItemId)
        REFERENCES dbo.FoodItem (Id)
);
GO

-- ============================================================
-- 3. EXAMS
-- ============================================================

CREATE TABLE dbo.ExamCategory (
    Id          UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    Name        NVARCHAR(100)    NOT NULL,
    SortOrder   TINYINT          NOT NULL DEFAULT 0,
    IsActive    BIT              NOT NULL DEFAULT 1,

    CONSTRAINT PK_ExamCategory PRIMARY KEY (Id)
);
GO

CREATE TABLE dbo.Exam (
    Id              UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    ExamCategoryId  UNIQUEIDENTIFIER NOT NULL,
    Name            NVARCHAR(150)    NOT NULL,
    Abbreviation    NVARCHAR(50)     NULL,
    Description     NVARCHAR(300)    NULL,
    IsActive        BIT              NOT NULL DEFAULT 1,

    CONSTRAINT PK_Exam              PRIMARY KEY (Id),
    CONSTRAINT FK_Exam_Category     FOREIGN KEY (ExamCategoryId)
        REFERENCES dbo.ExamCategory (Id)
);
GO

CREATE TABLE dbo.ExamRequest (
    Id              UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    UserId          UNIQUEIDENTIFIER NOT NULL,
    RequestDate     DATE             NOT NULL DEFAULT CAST(GETUTCDATE() AS DATE),
    DoctorName      NVARCHAR(150)    NULL,
    Notes           NVARCHAR(500)    NULL,
    CreatedAt       DATETIME2        NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt       DATETIME2        NULL,

    CONSTRAINT PK_ExamRequest           PRIMARY KEY (Id),
    CONSTRAINT FK_ExamRequest_User      FOREIGN KEY (UserId)
        REFERENCES dbo.Users (Id)
);
GO

CREATE TABLE dbo.ExamRequestItem (
    Id              UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    ExamRequestId   UNIQUEIDENTIFIER NOT NULL,
    ExamId          UNIQUEIDENTIFIER NOT NULL,
    IsCompleted     BIT              NOT NULL DEFAULT 0,
    CompletedDate   DATE             NULL,
    Result          NVARCHAR(500)    NULL,
    Laboratory      NVARCHAR(150)    NULL,
    Notes           NVARCHAR(300)    NULL,

    CONSTRAINT PK_ExamRequestItem               PRIMARY KEY (Id),
    CONSTRAINT FK_ExamRequestItem_ExamRequest   FOREIGN KEY (ExamRequestId)
        REFERENCES dbo.ExamRequest (Id),
    CONSTRAINT FK_ExamRequestItem_Exam          FOREIGN KEY (ExamId)
        REFERENCES dbo.Exam (Id),
    CONSTRAINT UQ_ExamRequestItem               UNIQUE (ExamRequestId, ExamId)
);
GO

-- ============================================================
-- INDEXES
-- ============================================================
CREATE INDEX IX_FoodItem_Category          ON dbo.FoodItem (FoodCategoryId);
CREATE INDEX IX_DailyLog_User              ON dbo.DailyLog (UserId);
CREATE INDEX IX_DailyLog_Date              ON dbo.DailyLog (LogDate);
CREATE INDEX IX_DailyLogItem_DailyLog      ON dbo.DailyLogItem (DailyLogId);
CREATE INDEX IX_Exam_Category              ON dbo.Exam (ExamCategoryId);
CREATE INDEX IX_ExamRequest_User           ON dbo.ExamRequest (UserId);
CREATE INDEX IX_ExamRequestItem_Request    ON dbo.ExamRequestItem (ExamRequestId);
GO

PRINT 'DDL executed successfully — all PKs are UNIQUEIDENTIFIER.';
GO
