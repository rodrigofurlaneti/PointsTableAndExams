-- ============================================================
-- POINTS TABLE AND EXAMS SYSTEM
-- Database Modeling: SQL Server
-- Standard: US English naming conventions
-- Date: 2026-05-31
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

-- ============================================================
-- 1. USERS
-- ============================================================
IF OBJECT_ID('dbo.Users', 'U') IS NOT NULL DROP TABLE dbo.Users;

CREATE TABLE dbo.Users (
    Id              INT             NOT NULL IDENTITY(1,1),
    FullName        NVARCHAR(150)   NOT NULL,
    Email           NVARCHAR(150)   NOT NULL,
    PhoneNumber     NVARCHAR(20)    NULL,
    BirthDate       DATE            NULL,
    Gender          CHAR(1)         NOT NULL,   -- 'M' | 'F'
    Username        NVARCHAR(80)    NOT NULL,
    PasswordHash    NVARCHAR(256)   NOT NULL,   -- store bcrypt/SHA-256 hash
    IsActive        BIT             NOT NULL    DEFAULT 1,
    CreatedAt       DATETIME2       NOT NULL    DEFAULT GETDATE(),
    UpdatedAt       DATETIME2       NULL,

    CONSTRAINT PK_Users             PRIMARY KEY (Id),
    CONSTRAINT UQ_Users_Email       UNIQUE (Email),
    CONSTRAINT UQ_Users_Username    UNIQUE (Username),
    CONSTRAINT CK_Users_Gender      CHECK (Gender IN ('M', 'F', 'O'))
);
GO

-- ============================================================
-- 2. FOOD POINTS TABLE
-- ============================================================

-- 2.1 Food category (Vegetables, Legumes, Meats, Cheeses, etc.)
IF OBJECT_ID('dbo.FoodCategory', 'U') IS NOT NULL DROP TABLE dbo.FoodCategory;

CREATE TABLE dbo.FoodCategory (
    Id                  INT             NOT NULL IDENTITY(1,1),
    Name                NVARCHAR(100)   NOT NULL,
    Description         NVARCHAR(300)   NULL,
    -- Default points per serving quota (NULL = calculated per food item)
    DefaultQuotaPoints  SMALLINT        NULL,
    -- Serving unit description (e.g., "2 tablespoons", "1 unit")
    ServingUnit         NVARCHAR(100)   NULL,
    SortOrder           TINYINT         NOT NULL DEFAULT 0,
    IsActive            BIT             NOT NULL DEFAULT 1,

    CONSTRAINT PK_FoodCategory PRIMARY KEY (Id)
);
GO

-- 2.2 Food items
IF OBJECT_ID('dbo.FoodItem', 'U') IS NOT NULL DROP TABLE dbo.FoodItem;

CREATE TABLE dbo.FoodItem (
    Id              INT             NOT NULL IDENTITY(1,1),
    FoodCategoryId  INT             NOT NULL,
    Name            NVARCHAR(150)   NOT NULL,
    -- Serving description (e.g., "1 medium unit", "2 tablespoons")
    ServingSize     NVARCHAR(100)   NULL,
    -- Points for this serving (overrides category default when provided)
    Points          SMALLINT        NOT NULL DEFAULT 0,
    Notes           NVARCHAR(300)   NULL,
    IsActive        BIT             NOT NULL DEFAULT 1,

    CONSTRAINT PK_FoodItem              PRIMARY KEY (Id),
    CONSTRAINT FK_FoodItem_Category     FOREIGN KEY (FoodCategoryId)
        REFERENCES dbo.FoodCategory (Id)
);
GO

-- 2.3 Daily food intake log
IF OBJECT_ID('dbo.DailyLog', 'U') IS NOT NULL DROP TABLE dbo.DailyLog;

CREATE TABLE dbo.DailyLog (
    Id          INT             NOT NULL IDENTITY(1,1),
    UserId      INT             NOT NULL,
    LogDate     DATE            NOT NULL,
    TotalPoints SMALLINT        NOT NULL DEFAULT 0,
    Notes       NVARCHAR(500)   NULL,
    CreatedAt   DATETIME2       NOT NULL DEFAULT GETDATE(),
    UpdatedAt   DATETIME2       NULL,

    CONSTRAINT PK_DailyLog              PRIMARY KEY (Id),
    CONSTRAINT FK_DailyLog_User         FOREIGN KEY (UserId)
        REFERENCES dbo.Users (Id),
    CONSTRAINT UQ_DailyLog_UserDate     UNIQUE (UserId, LogDate)
);
GO

-- 2.4 Daily log line items
IF OBJECT_ID('dbo.DailyLogItem', 'U') IS NOT NULL DROP TABLE dbo.DailyLogItem;

CREATE TABLE dbo.DailyLogItem (
    Id              INT             NOT NULL IDENTITY(1,1),
    DailyLogId      INT             NOT NULL,
    FoodItemId      INT             NOT NULL,
    Quantity        DECIMAL(5,2)    NOT NULL DEFAULT 1,   -- number of servings
    PointsComputed  SMALLINT        NOT NULL DEFAULT 0,
    MealTime        TIME            NULL,                  -- time of consumption
    Notes           NVARCHAR(200)   NULL,

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

-- 3.1 Exam category (Biochemistry, Immunology, Hematology, etc.)
IF OBJECT_ID('dbo.ExamCategory', 'U') IS NOT NULL DROP TABLE dbo.ExamCategory;

CREATE TABLE dbo.ExamCategory (
    Id          INT             NOT NULL IDENTITY(1,1),
    Name        NVARCHAR(100)   NOT NULL,
    SortOrder   TINYINT         NOT NULL DEFAULT 0,
    IsActive    BIT             NOT NULL DEFAULT 1,

    CONSTRAINT PK_ExamCategory PRIMARY KEY (Id)
);
GO

-- 3.2 Available exams
IF OBJECT_ID('dbo.Exam', 'U') IS NOT NULL DROP TABLE dbo.Exam;

CREATE TABLE dbo.Exam (
    Id              INT             NOT NULL IDENTITY(1,1),
    ExamCategoryId  INT             NOT NULL,
    Name            NVARCHAR(150)   NOT NULL,
    Abbreviation    NVARCHAR(50)    NULL,
    Description     NVARCHAR(300)   NULL,
    IsActive        BIT             NOT NULL DEFAULT 1,

    CONSTRAINT PK_Exam              PRIMARY KEY (Id),
    CONSTRAINT FK_Exam_Category     FOREIGN KEY (ExamCategoryId)
        REFERENCES dbo.ExamCategory (Id)
);
GO

-- 3.3 Exam requests per user
IF OBJECT_ID('dbo.ExamRequest', 'U') IS NOT NULL DROP TABLE dbo.ExamRequest;

CREATE TABLE dbo.ExamRequest (
    Id              INT             NOT NULL IDENTITY(1,1),
    UserId          INT             NOT NULL,
    RequestDate     DATE            NOT NULL DEFAULT CAST(GETDATE() AS DATE),
    DoctorName      NVARCHAR(150)   NULL,
    Notes           NVARCHAR(500)   NULL,
    CreatedAt       DATETIME2       NOT NULL DEFAULT GETDATE(),

    CONSTRAINT PK_ExamRequest           PRIMARY KEY (Id),
    CONSTRAINT FK_ExamRequest_User      FOREIGN KEY (UserId)
        REFERENCES dbo.Users (Id)
);
GO

-- 3.4 Exam request line items
IF OBJECT_ID('dbo.ExamRequestItem', 'U') IS NOT NULL DROP TABLE dbo.ExamRequestItem;

CREATE TABLE dbo.ExamRequestItem (
    Id              INT             NOT NULL IDENTITY(1,1),
    ExamRequestId   INT             NOT NULL,
    ExamId          INT             NOT NULL,
    IsCompleted     BIT             NOT NULL DEFAULT 0,
    CompletedDate   DATE            NULL,
    Result          NVARCHAR(500)   NULL,
    Laboratory      NVARCHAR(150)   NULL,
    Notes           NVARCHAR(300)   NULL,

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

PRINT 'DDL executed successfully.';
GO
