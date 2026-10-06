USE PublicLibrary;
GO

CREATE OR ALTER PROCEDURE dbo.GetBooksByBorrowedCount
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Id AS BookId,
        Name AS BookName,
        TotalBorrowed
    FROM dbo.Book
    WHERE IsDeleted = 0
      AND IsActive = 1
    ORDER BY TotalBorrowed DESC;
END
GO


CREATE OR ALTER PROCEDURE dbo.GetUserLendingRate
    @FromDate DATE,
    @ToDate DATE
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        b.Id AS BorrowerId,
        b.Name,
        COUNT(l.Id) AS BorrowCount
    FROM dbo.Borrower b
    INNER JOIN dbo.Lending l
        ON b.Id = l.BorrowerId
    WHERE l.LendingDate >= @FromDate
      AND l.LendingDate < DATEADD(DAY, 1, @ToDate)
      AND b.IsDeleted = 0
      AND b.IsActive = 1
    GROUP BY
        b.Id,
        b.Name
    ORDER BY
        BorrowCount DESC;
END
GO


CREATE OR ALTER PROCEDURE dbo.GetUserReadingPace
    @BorrowerId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        br.Id AS BorrowerId,
        b.Name as BookName,
        l.ToDate as ReturnDate,        
        CAST(b.TotalPages/DATEDIFF(DAY, l.FromDate, ISNULL(l.ToDate, CAST(GETDATE() AS DATE))) AS DECIMAL(10,2)) AS ReadingPace
    FROM dbo.Lending l
    INNER JOIN dbo.Book b
        ON l.BookId = b.Id
    INNER JOIN dbo.Borrower br
        ON l.BorrowerId = br.Id
    WHERE l.BorrowerId = @BorrowerId
      AND b.IsDeleted = 0
      AND b.IsActive = 1
      AND br.IsDeleted = 0
      AND br.IsActive = 1    
END
GO



CREATE OR ALTER PROCEDURE dbo.GetUserBorrowingPatterns
    @BookId INT
AS
BEGIN
    SET NOCOUNT ON;

    select Id,Name from Book where Id in (
    select DISTINCT BookId from Lending where BorrowerId in (
    select BorrowerId from Lending
    where BookId = @BookId)) and IsActive = 1 and IsDeleted = 0;
END
GO
