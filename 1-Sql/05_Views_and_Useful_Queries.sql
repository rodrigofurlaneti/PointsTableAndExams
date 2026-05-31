-- ============================================================
-- VIEWS AND USEFUL QUERIES
-- ============================================================

USE PointsTableAndExams;
GO

-- ============================================================
-- VIEW: Detailed daily log per user
-- ============================================================
CREATE OR ALTER VIEW dbo.vw_DailyLogDetailed AS
SELECT
    u.Id                AS UserId,
    u.FullName,
    dl.LogDate,
    dl.TotalPoints,
    fc.Name             AS FoodCategory,
    fi.Name             AS FoodItem,
    fi.ServingSize,
    dli.Quantity,
    dli.PointsComputed,
    dli.MealTime
FROM   dbo.DailyLog     dl
JOIN   dbo.Users        u   ON u.Id  = dl.UserId
JOIN   dbo.DailyLogItem dli ON dli.DailyLogId  = dl.Id
JOIN   dbo.FoodItem     fi  ON fi.Id = dli.FoodItemId
JOIN   dbo.FoodCategory fc  ON fc.Id = fi.FoodCategoryId;
GO

-- ============================================================
-- VIEW: Daily points history (summary)
-- ============================================================
CREATE OR ALTER VIEW dbo.vw_DailyPointsHistory AS
SELECT
    u.Id                AS UserId,
    u.FullName,
    dl.LogDate,
    dl.TotalPoints,
    COUNT(dli.Id)       AS FoodItemCount
FROM   dbo.DailyLog         dl
JOIN   dbo.Users            u   ON u.Id = dl.UserId
LEFT   JOIN dbo.DailyLogItem dli ON dli.DailyLogId = dl.Id
GROUP  BY u.Id, u.FullName, dl.LogDate, dl.TotalPoints;
GO

-- ============================================================
-- VIEW: Exams per user with status
-- ============================================================
CREATE OR ALTER VIEW dbo.vw_ExamsByUser AS
SELECT
    u.Id                AS UserId,
    u.FullName,
    er.RequestDate,
    er.DoctorName,
    ec.Name             AS ExamCategory,
    e.Name              AS ExamName,
    e.Abbreviation,
    eri.IsCompleted,
    eri.CompletedDate,
    eri.Result,
    eri.Laboratory
FROM   dbo.ExamRequest      er
JOIN   dbo.Users            u   ON u.Id   = er.UserId
JOIN   dbo.ExamRequestItem  eri ON eri.ExamRequestId = er.Id
JOIN   dbo.Exam             e   ON e.Id   = eri.ExamId
JOIN   dbo.ExamCategory     ec  ON ec.Id  = e.ExamCategoryId;
GO

-- ============================================================
-- USEFUL QUERIES (commented out — ready to use)
-- ============================================================

-- Points consumed by a user in the last 7 days:
-- SELECT FullName, LogDate, TotalPoints
-- FROM   dbo.vw_DailyPointsHistory
-- WHERE  UserId = <ID>
--   AND  LogDate >= CAST(DATEADD(DAY, -7, GETDATE()) AS DATE)
-- ORDER  BY LogDate DESC;

-- Pending exams (not yet completed) for a user:
-- SELECT FullName, RequestDate, ExamCategory, ExamName, Abbreviation
-- FROM   dbo.vw_ExamsByUser
-- WHERE  UserId = <ID>
--   AND  IsCompleted = 0
-- ORDER  BY ExamCategory, ExamName;

-- Points breakdown by food category on a specific day:
-- SELECT fc.Name AS Category, SUM(dli.PointsComputed * dli.Quantity) AS TotalPoints
-- FROM   dbo.DailyLog         dl
-- JOIN   dbo.DailyLogItem     dli ON dli.DailyLogId  = dl.Id
-- JOIN   dbo.FoodItem         fi  ON fi.Id = dli.FoodItemId
-- JOIN   dbo.FoodCategory     fc  ON fc.Id = fi.FoodCategoryId
-- WHERE  dl.UserId  = <ID>
--   AND  dl.LogDate = '<DATE>'
-- GROUP  BY fc.Name
-- ORDER  BY TotalPoints DESC;

-- All available exams grouped by category:
-- SELECT ec.Name AS Category, e.Name AS ExamName, e.Abbreviation, e.Description
-- FROM   dbo.Exam         e
-- JOIN   dbo.ExamCategory ec ON ec.Id = e.ExamCategoryId
-- WHERE  e.IsActive = 1
-- ORDER  BY ec.SortOrder, e.Name;

GO

PRINT 'Views created successfully.';
GO
