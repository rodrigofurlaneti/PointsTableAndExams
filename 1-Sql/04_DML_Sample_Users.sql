-- ============================================================
-- SEED: Sample Users, Daily Logs and Exam Requests
-- PKs: UNIQUEIDENTIFIER — SCOPE_IDENTITY() not used (no IDENTITY)
-- ============================================================

USE PointsTableAndExams;
GO

SET NOCOUNT ON;

-- ============================================================
-- SAMPLE USERS
-- BCrypt hash of "password" (workFactor 10)
-- ============================================================
INSERT INTO dbo.Users (Id, FullName, Email, PhoneNumber, BirthDate, Gender, Username, PasswordHash) VALUES
(NEWID(), 'Ana Paula Souza',      'ana.souza@email.com',     '11999990001', '1990-03-15', 'Female', 'ana.souza',   '$2a$10$N9qo8uLOickgx2ZMRZoMyeIjZAgcfl7p92ldGxad68LJZdL17lhWy'),
(NEWID(), 'Carlos Henrique Lima', 'carlos.lima@email.com',   '21988880002', '1985-07-22', 'Male',   'carlos.lima', '$2a$10$N9qo8uLOickgx2ZMRZoMyeIjZAgcfl7p92ldGxad68LJZdL17lhWy'),
(NEWID(), 'Fernanda Costa',       'fernanda.costa@email.com','31977770003', '1995-11-08', 'Female', 'fernanda.c',  '$2a$10$N9qo8uLOickgx2ZMRZoMyeIjZAgcfl7p92ldGxad68LJZdL17lhWy');
GO

-- ============================================================
-- SAMPLE DAILY LOG — Ana Paula (2026-05-31)
-- ============================================================
DECLARE @userId  UNIQUEIDENTIFIER = (SELECT Id FROM dbo.Users WHERE Username = 'ana.souza');
DECLARE @logId   UNIQUEIDENTIFIER = NEWID();

INSERT INTO dbo.DailyLog (Id, UserId, LogDate, TotalPoints, Notes)
VALUES (@logId, @userId, '2026-05-31', 0, 'First sample log entry');

INSERT INTO dbo.DailyLogItem (Id, DailyLogId, FoodItemId, Quantity, PointsComputed, MealTime)
SELECT NEWID(), @logId, Id, 1, Points, '07:30' FROM dbo.FoodItem WHERE Name = 'Cooked White Rice';

INSERT INTO dbo.DailyLogItem (Id, DailyLogId, FoodItemId, Quantity, PointsComputed, MealTime)
SELECT NEWID(), @logId, Id, 1, Points, '07:30' FROM dbo.FoodItem WHERE Name = 'Cooked Black Beans';

INSERT INTO dbo.DailyLogItem (Id, DailyLogId, FoodItemId, Quantity, PointsComputed, MealTime)
SELECT NEWID(), @logId, Id, 1, Points, '07:30' FROM dbo.FoodItem WHERE Name = 'Lean Steak (no fat/skin)';

INSERT INTO dbo.DailyLogItem (Id, DailyLogId, FoodItemId, Quantity, PointsComputed, MealTime)
SELECT NEWID(), @logId, Id, 1, Points, '15:00' FROM dbo.FoodItem WHERE Name = 'Apple';

-- Recalculate daily total
UPDATE dbo.DailyLog
SET    TotalPoints = (
    SELECT ISNULL(SUM(CAST(PointsComputed AS INT) * CAST(Quantity AS INT)), 0)
    FROM   dbo.DailyLogItem
    WHERE  DailyLogId = @logId
)
WHERE  Id = @logId;
GO

-- ============================================================
-- SAMPLE EXAM REQUEST — Carlos (2026-05-31)
-- ============================================================
DECLARE @userId2 UNIQUEIDENTIFIER = (SELECT Id FROM dbo.Users WHERE Username = 'carlos.lima');
DECLARE @reqId   UNIQUEIDENTIFIER = NEWID();

INSERT INTO dbo.ExamRequest (Id, UserId, RequestDate, DoctorName, Notes)
VALUES (@reqId, @userId2, '2026-05-31', 'Dr. Sinval de Oliveira Muniz Jr.', 'Routine endocrinology checkup');

INSERT INTO dbo.ExamRequestItem (Id, ExamRequestId, ExamId)
SELECT NEWID(), @reqId, Id FROM dbo.Exam WHERE Name IN (
    'Fasting Glucose',
    'Glycated Hemoglobin',
    'Total Cholesterol and Fractions',
    'Triglycerides',
    'TSH',
    'Free T4',
    'Insulin',
    'Complete Blood Count',
    'Creatinine',
    'AST',
    'ALT'
);
GO

PRINT 'Sample data inserted successfully.';
GO
