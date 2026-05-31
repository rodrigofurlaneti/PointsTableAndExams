-- ============================================================
-- SEED: Sample Users, Daily Logs and Exam Requests
-- NOTE: PasswordHash must store a real hash (bcrypt/SHA-256).
--       Values below are illustrative only.
-- ============================================================

USE PointsTableAndExams;
GO

SET NOCOUNT ON;

-- ============================================================
-- SAMPLE USERS
-- ============================================================
INSERT INTO dbo.Users (FullName, Email, PhoneNumber, BirthDate, Gender, Username, PasswordHash) VALUES
('Ana Paula Souza',      'ana.souza@email.com',     '11999990001', '1990-03-15', 'F', 'ana.souza',    '5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8'),
('Carlos Henrique Lima', 'carlos.lima@email.com',   '21988880002', '1985-07-22', 'M', 'carlos.lima',  '5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8'),
('Fernanda Costa',       'fernanda.costa@email.com','31977770003', '1995-11-08', 'F', 'fernanda.c',   '5e884898da28047151d0e56f8dc6292773603d0d6aabbdd62a11ef721d1542d8');

-- ============================================================
-- SAMPLE DAILY LOG — Ana Paula (2026-05-31)
-- ============================================================
DECLARE @userId INT = (SELECT Id FROM dbo.Users WHERE Username = 'ana.souza');

INSERT INTO dbo.DailyLog (UserId, LogDate, TotalPoints, Notes)
VALUES (@userId, '2026-05-31', 0, 'First sample log entry');

DECLARE @logId INT = SCOPE_IDENTITY();

INSERT INTO dbo.DailyLogItem (DailyLogId, FoodItemId, Quantity, PointsComputed, MealTime)
SELECT @logId, Id, 1, Points, '07:30' FROM dbo.FoodItem WHERE Name = 'Cooked White Rice';

INSERT INTO dbo.DailyLogItem (DailyLogId, FoodItemId, Quantity, PointsComputed, MealTime)
SELECT @logId, Id, 1, Points, '07:30' FROM dbo.FoodItem WHERE Name = 'Cooked Black Beans';

INSERT INTO dbo.DailyLogItem (DailyLogId, FoodItemId, Quantity, PointsComputed, MealTime)
SELECT @logId, Id, 1, Points, '07:30' FROM dbo.FoodItem WHERE Name = 'Lean Steak (no fat/skin)';

INSERT INTO dbo.DailyLogItem (DailyLogId, FoodItemId, Quantity, PointsComputed, MealTime)
SELECT @logId, Id, 1, Points, '15:00' FROM dbo.FoodItem WHERE Name = 'Apple';

-- Recalculate daily total
UPDATE dbo.DailyLog
SET    TotalPoints = (
    SELECT ISNULL(SUM(PointsComputed * Quantity), 0)
    FROM   dbo.DailyLogItem
    WHERE  DailyLogId = @logId
)
WHERE  Id = @logId;

-- ============================================================
-- SAMPLE EXAM REQUEST — Carlos (2026-05-31)
-- ============================================================
DECLARE @userId2 INT = (SELECT Id FROM dbo.Users WHERE Username = 'carlos.lima');

INSERT INTO dbo.ExamRequest (UserId, RequestDate, DoctorName, Notes)
VALUES (@userId2, '2026-05-31', 'Dr. Sinval de Oliveira Muniz Jr.', 'Routine endocrinology checkup');

DECLARE @reqId INT = SCOPE_IDENTITY();

INSERT INTO dbo.ExamRequestItem (ExamRequestId, ExamId)
SELECT @reqId, Id FROM dbo.Exam WHERE Name IN (
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
