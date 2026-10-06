```sql
-- =============================================
-- Public Library - Test Data
-- SQL Server
-- =============================================

USE PublicLibrary;
GO

SET NOCOUNT ON;
GO


-- =============================================
-- 1. CLEAR EXISTING DATA
-- =============================================

DELETE FROM dbo.Lending;
DELETE FROM dbo.Borrower;
DELETE FROM dbo.Book;

DBCC CHECKIDENT ('dbo.Lending', RESEED, 0);
DBCC CHECKIDENT ('dbo.Borrower', RESEED, 0);
DBCC CHECKIDENT ('dbo.Book', RESEED, 0);
GO


-- =============================================
-- 2. BOOK TEST DATA
-- =============================================

INSERT INTO dbo.Book
(
    Name,
    Author,
    TotalPages,
    Price,
    TotalCount,
    TotalBorrowed,
    IsActive,
    IsDeleted
)
VALUES
('The Great Gatsby', 'F. Scott Fitzgerald', 180, 299.00, 5, 0, 1, 0),
('To Kill a Mockingbird', 'Harper Lee', 281, 399.00, 4, 0, 1, 0),
('1984', 'George Orwell', 328, 349.00, 6, 0, 1, 0),
('Pride and Prejudice', 'Jane Austen', 432, 299.00, 5, 0, 1, 0),
('The Hobbit', 'J.R.R. Tolkien', 310, 499.00, 7, 0, 1, 0),
('Harry Potter and the Philosopher''s Stone', 'J.K. Rowling', 309, 599.00, 8, 0, 1, 0),
('The Alchemist', 'Paulo Coelho', 208, 299.00, 5, 0, 1, 0),
('Atomic Habits', 'James Clear', 320, 599.00, 10, 0, 1, 0),
('The Psychology of Money', 'Morgan Housel', 256, 499.00, 6, 0, 1, 0),
('Clean Code', 'Robert C. Martin', 464, 799.00, 5, 0, 1, 0),
('Design Patterns', 'Erich Gamma', 395, 899.00, 4, 0, 1, 0),
('The Pragmatic Programmer', 'Andrew Hunt', 352, 749.00, 5, 0, 1, 0),
('Deep Work', 'Cal Newport', 304, 499.00, 6, 0, 1, 0),
('Rich Dad Poor Dad', 'Robert Kiyosaki', 336, 399.00, 7, 0, 1, 0),
('The Lean Startup', 'Eric Ries', 336, 549.00, 4, 0, 1, 0),
('Sapiens', 'Yuval Noah Harari', 498, 699.00, 6, 0, 1, 0),
('Thinking, Fast and Slow', 'Daniel Kahneman', 512, 649.00, 5, 0, 1, 0),
('The Power of Habit', 'Charles Duhigg', 371, 449.00, 5, 0, 1, 0),
('The 7 Habits of Highly Effective People', 'Stephen Covey', 381, 499.00, 6, 0, 1, 0),
('Refactoring', 'Martin Fowler', 448, 849.00, 4, 0, 1, 0);
GO


-- =============================================
-- 3. BORROWER TEST DATA
-- =============================================

INSERT INTO dbo.Borrower
(
    Name,
    IsActive,
    IsDeleted
)
VALUES
('Amit Sharma', 1, 0),
('Priya Mehta', 1, 0),
('Rahul Verma', 1, 0),
('Sneha Kapoor', 1, 0),
('Arjun Malhotra', 1, 0),
('Neha Gupta', 1, 0),
('Vikram Singh', 1, 0),
('Ananya Joshi', 1, 0),
('Rohan Patel', 1, 0),
('Kavya Nair', 1, 0);
GO


-- =============================================
-- 4. CURRENT LENDING
--
-- Approximately 50% of each book inventory
-- is currently borrowed.
--
-- ToDate is ALWAYS populated.
-- Future ToDate = currently borrowed.
-- =============================================

INSERT INTO dbo.Lending
(
    BorrowerId,
    BookId,
    FromDate,
    ToDate,
    LendingDate
)
VALUES

-- =============================================
-- BOOK 1
-- Inventory = 5
-- Current = 3
-- =============================================

(1,1,
 DATEADD(DAY,-10,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,10,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-10,SYSUTCDATETIME())),

(2,1,
 DATEADD(DAY,-8,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,12,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-8,SYSUTCDATETIME())),

(3,1,
 DATEADD(DAY,-5,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,15,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-5,SYSUTCDATETIME())),


-- =============================================
-- BOOK 2
-- Inventory = 4
-- Current = 2
-- =============================================

(4,2,
 DATEADD(DAY,-12,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,8,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-12,SYSUTCDATETIME())),

(5,2,
 DATEADD(DAY,-6,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,14,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-6,SYSUTCDATETIME())),


-- =============================================
-- BOOK 3
-- Inventory = 6
-- Current = 3
-- =============================================

(6,3,
 DATEADD(DAY,-15,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,15,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-15,SYSUTCDATETIME())),

(7,3,
 DATEADD(DAY,-10,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,20,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-10,SYSUTCDATETIME())),

(8,3,
 DATEADD(DAY,-4,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,18,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-4,SYSUTCDATETIME())),


-- =============================================
-- BOOK 4
-- Inventory = 5
-- Current = 3
-- =============================================

(9,4,
 DATEADD(DAY,-11,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,9,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-11,SYSUTCDATETIME())),

(10,4,
 DATEADD(DAY,-7,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,13,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-7,SYSUTCDATETIME())),

(1,4,
 DATEADD(DAY,-3,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,17,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-3,SYSUTCDATETIME())),


-- =============================================
-- BOOK 5
-- Inventory = 7
-- Current = 4
-- =============================================

(2,5,
 DATEADD(DAY,-14,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,16,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-14,SYSUTCDATETIME())),

(3,5,
 DATEADD(DAY,-9,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,21,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-9,SYSUTCDATETIME())),

(4,5,
 DATEADD(DAY,-6,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,14,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-6,SYSUTCDATETIME())),

(5,5,
 DATEADD(DAY,-2,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,19,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-2,SYSUTCDATETIME())),


-- =============================================
-- BOOK 6
-- Inventory = 8
-- Current = 4
-- =============================================

(6,6,
 DATEADD(DAY,-18,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,12,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-18,SYSUTCDATETIME())),

(7,6,
 DATEADD(DAY,-13,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,17,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-13,SYSUTCDATETIME())),

(8,6,
 DATEADD(DAY,-8,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,22,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-8,SYSUTCDATETIME())),

(9,6,
 DATEADD(DAY,-4,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,26,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-4,SYSUTCDATETIME())),


-- =============================================
-- BOOK 7
-- Inventory = 5
-- Current = 3
-- =============================================

(10,7,
 DATEADD(DAY,-10,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,20,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-10,SYSUTCDATETIME())),

(1,7,
 DATEADD(DAY,-7,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,23,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-7,SYSUTCDATETIME())),

(2,7,
 DATEADD(DAY,-3,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,27,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-3,SYSUTCDATETIME())),


-- =============================================
-- BOOK 8
-- Inventory = 10
-- Current = 5
-- =============================================

(3,8,
 DATEADD(DAY,-20,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,10,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-20,SYSUTCDATETIME())),

(4,8,
 DATEADD(DAY,-16,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,14,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-16,SYSUTCDATETIME())),

(5,8,
 DATEADD(DAY,-12,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,18,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-12,SYSUTCDATETIME())),

(6,8,
 DATEADD(DAY,-8,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,22,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-8,SYSUTCDATETIME())),

(7,8,
 DATEADD(DAY,-4,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,26,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-4,SYSUTCDATETIME())),


-- =============================================
-- BOOK 9
-- Inventory = 6
-- Current = 3
-- =============================================

(8,9,
 DATEADD(DAY,-15,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,15,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-15,SYSUTCDATETIME())),

(9,9,
 DATEADD(DAY,-9,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,21,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-9,SYSUTCDATETIME())),

(10,9,
 DATEADD(DAY,-5,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,25,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-5,SYSUTCDATETIME())),


-- =============================================
-- BOOK 10
-- Inventory = 5
-- Current = 3
-- =============================================

(1,10,
 DATEADD(DAY,-12,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,18,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-12,SYSUTCDATETIME())),

(2,10,
 DATEADD(DAY,-8,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,22,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-8,SYSUTCDATETIME())),

(3,10,
 DATEADD(DAY,-3,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,27,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-3,SYSUTCDATETIME())),


-- =============================================
-- BOOK 11
-- Inventory = 4
-- Current = 2
-- =============================================

(4,11,
 DATEADD(DAY,-14,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,16,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-14,SYSUTCDATETIME())),

(5,11,
 DATEADD(DAY,-6,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,24,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-6,SYSUTCDATETIME())),


-- =============================================
-- BOOK 12
-- Inventory = 5
-- Current = 3
-- =============================================

(6,12,
 DATEADD(DAY,-11,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,19,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-11,SYSUTCDATETIME())),

(7,12,
 DATEADD(DAY,-7,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,23,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-7,SYSUTCDATETIME())),

(8,12,
 DATEADD(DAY,-2,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,28,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-2,SYSUTCDATETIME())),


-- =============================================
-- BOOK 13
-- Inventory = 6
-- Current = 3
-- =============================================

(9,13,
 DATEADD(DAY,-16,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,14,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-16,SYSUTCDATETIME())),

(10,13,
 DATEADD(DAY,-9,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,21,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-9,SYSUTCDATETIME())),

(1,13,
 DATEADD(DAY,-4,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,26,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-4,SYSUTCDATETIME())),


-- =============================================
-- BOOK 14
-- Inventory = 7
-- Current = 4
-- =============================================

(2,14,
 DATEADD(DAY,-18,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,12,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-18,SYSUTCDATETIME())),

(3,14,
 DATEADD(DAY,-13,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,17,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-13,SYSUTCDATETIME())),

(4,14,
 DATEADD(DAY,-8,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,22,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-8,SYSUTCDATETIME())),

(5,14,
 DATEADD(DAY,-3,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,27,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-3,SYSUTCDATETIME())),


-- =============================================
-- BOOK 15
-- Inventory = 4
-- Current = 2
-- =============================================

(6,15,
 DATEADD(DAY,-12,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,18,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-12,SYSUTCDATETIME())),

(7,15,
 DATEADD(DAY,-5,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,25,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-5,SYSUTCDATETIME())),


-- =============================================
-- BOOK 16
-- Inventory = 6
-- Current = 3
-- =============================================

(8,16,
 DATEADD(DAY,-17,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,13,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-17,SYSUTCDATETIME())),

(9,16,
 DATEADD(DAY,-10,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,20,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-10,SYSUTCDATETIME())),

(10,16,
 DATEADD(DAY,-4,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,26,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-4,SYSUTCDATETIME())),


-- =============================================
-- BOOK 17
-- Inventory = 5
-- Current = 3
-- =============================================

(1,17,
 DATEADD(DAY,-14,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,16,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-14,SYSUTCDATETIME())),

(2,17,
 DATEADD(DAY,-8,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,22,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-8,SYSUTCDATETIME())),

(3,17,
 DATEADD(DAY,-3,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,27,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-3,SYSUTCDATETIME())),


-- =============================================
-- BOOK 18
-- Inventory = 5
-- Current = 3
-- =============================================

(4,18,
 DATEADD(DAY,-16,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,14,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-16,SYSUTCDATETIME())),

(5,18,
 DATEADD(DAY,-9,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,21,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-9,SYSUTCDATETIME())),

(6,18,
 DATEADD(DAY,-4,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,26,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-4,SYSUTCDATETIME())),


-- =============================================
-- BOOK 19
-- Inventory = 6
-- Current = 3
-- =============================================

(7,19,
 DATEADD(DAY,-15,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,15,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-15,SYSUTCDATETIME())),

(8,19,
 DATEADD(DAY,-10,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,20,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-10,SYSUTCDATETIME())),

(9,19,
 DATEADD(DAY,-5,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,25,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-5,SYSUTCDATETIME())),


-- =============================================
-- BOOK 20
-- Inventory = 4
-- Current = 2
-- =============================================

(10,20,
 DATEADD(DAY,-12,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,18,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-12,SYSUTCDATETIME())),

(1,20,
 DATEADD(DAY,-5,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,25,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-5,SYSUTCDATETIME()));
GO


-- =============================================
-- 5. HISTORICAL LENDING RECORDS
--
-- These provide previous borrowing activity
-- for lending-rate / reading-pace calculations.
--
-- ToDate is ALWAYS populated and in the past.
-- =============================================

INSERT INTO dbo.Lending
(
    BorrowerId,
    BookId,
    FromDate,
    ToDate,
    LendingDate
)
VALUES

(2,1,
 DATEADD(DAY,-150,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-125,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-150,SYSUTCDATETIME())),

(4,1,
 DATEADD(DAY,-210,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-180,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-210,SYSUTCDATETIME())),

(6,2,
 DATEADD(DAY,-145,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-115,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-145,SYSUTCDATETIME())),

(8,2,
 DATEADD(DAY,-200,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-170,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-200,SYSUTCDATETIME())),

(10,3,
 DATEADD(DAY,-130,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-95,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-130,SYSUTCDATETIME())),

(2,3,
 DATEADD(DAY,-220,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-190,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-220,SYSUTCDATETIME())),

(4,4,
 DATEADD(DAY,-140,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-105,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-140,SYSUTCDATETIME())),

(6,4,
 DATEADD(DAY,-230,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-200,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-230,SYSUTCDATETIME())),

(8,5,
 DATEADD(DAY,-160,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-125,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-160,SYSUTCDATETIME())),

(10,5,
 DATEADD(DAY,-240,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-210,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-240,SYSUTCDATETIME())),

(1,6,
 DATEADD(DAY,-155,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-120,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-155,SYSUTCDATETIME())),

(3,6,
 DATEADD(DAY,-250,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-220,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-250,SYSUTCDATETIME())),

(5,7,
 DATEADD(DAY,-135,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-100,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-135,SYSUTCDATETIME())),

(7,7,
 DATEADD(DAY,-225,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-195,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-225,SYSUTCDATETIME())),

(9,8,
 DATEADD(DAY,-170,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-135,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-170,SYSUTCDATETIME())),

(1,8,
 DATEADD(DAY,-260,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-230,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-260,SYSUTCDATETIME())),

(3,9,
 DATEADD(DAY,-125,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-90,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-125,SYSUTCDATETIME())),

(5,10,
 DATEADD(DAY,-145,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-110,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-145,SYSUTCDATETIME())),

(7,12,
 DATEADD(DAY,-180,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-145,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-180,SYSUTCDATETIME())),

(9,14,
 DATEADD(DAY,-190,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-155,CAST(GETDATE() AS DATE)),
 DATEADD(DAY,-190,SYSUTCDATETIME()));
GO


-- =============================================
-- 6. SYNCHRONIZE TotalBorrowed
--
-- Current borrowed = Lending records whose
-- ToDate is today or later.
-- =============================================

UPDATE b
SET b.TotalBorrowed = ISNULL(x.CurrentBorrowed, 0)
FROM dbo.Book b
LEFT JOIN
(
    SELECT
        BookId,
        COUNT(*) AS CurrentBorrowed
    FROM dbo.Lending
    WHERE ToDate >= CAST(GETDATE() AS DATE)
    GROUP BY BookId
) x
    ON b.Id = x.BookId;
GO


-- =============================================
-- 7. VALIDATION
-- =============================================

PRINT '=============================================';
PRINT 'TEST DATA CREATED';
PRINT '=============================================';

SELECT COUNT(*) AS TotalBooks
FROM dbo.Book;

SELECT COUNT(*) AS TotalBorrowers
FROM dbo.Borrower;

SELECT COUNT(*) AS TotalLendingRecords
FROM dbo.Lending;


-- =============================================
-- 8. INVENTORY VS LENDING
-- =============================================

SELECT
    b.Id AS BookId,
    b.Name,
    b.TotalCount,
    b.TotalBorrowed,

    COUNT(
        CASE
            WHEN l.ToDate >= CAST(GETDATE() AS DATE)
            THEN 1
        END
    ) AS ActualCurrentBorrowed,

    b.TotalCount - b.TotalBorrowed AS AvailableCount,

    CAST(
        b.TotalBorrowed * 100.0 / b.TotalCount
        AS DECIMAL(5,2)
    ) AS BorrowedPercentage,

    CASE
        WHEN b.TotalBorrowed =
             COUNT(
                 CASE
                     WHEN l.ToDate >= CAST(GETDATE() AS DATE)
                     THEN 1
                 END
             )
        AND b.TotalBorrowed <= b.TotalCount
        THEN 'OK'
        ELSE 'MISMATCH'
    END AS DataConsistency

FROM dbo.Book b

LEFT JOIN dbo.Lending l
    ON b.Id = l.BookId

GROUP BY
    b.Id,
    b.Name,
    b.TotalCount,
    b.TotalBorrowed

ORDER BY b.Id;
GO


-- =============================================
-- 9. VERIFY NO NULL ToDate
-- =============================================

SELECT
    COUNT(*) AS NullToDateCount
FROM dbo.Lending
WHERE ToDate IS NULL;
GO


-- =============================================
-- 10. CURRENT VS RETURNED
-- =============================================

SELECT
    CASE
        WHEN ToDate >= CAST(GETDATE() AS DATE)
            THEN 'Currently Borrowed'
        ELSE 'Returned'
    END AS LendingStatus,

    COUNT(*) AS LendingCount

FROM dbo.Lending

GROUP BY
    CASE
        WHEN ToDate >= CAST(GETDATE() AS DATE)
            THEN 'Currently Borrowed'
        ELSE 'Returned'
    END;
GO


-- =============================================
-- 11. CHECK FOR INVENTORY VIOLATIONS
-- =============================================

SELECT
    b.Id AS BookId,
    b.Name,
    b.TotalCount,
    b.TotalBorrowed
FROM dbo.Book b
WHERE b.TotalBorrowed > b.TotalCount;
GO
```
