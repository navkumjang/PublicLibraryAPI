using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;

namespace PublicLibrary.IntegrationTests.Support
{
    internal static class TestDatabase
    {
        public static string CreateTestDatabase(string dbName)
        {
            var masterConn = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Integrated Security=true;Initial Catalog=master");
            masterConn.Open();

            try
            {
                masterConn.Execute($"IF DB_ID('{dbName}') IS NOT NULL DROP DATABASE [{dbName}]");
                masterConn.Execute($"CREATE DATABASE [{dbName}]");
            }
            finally
            {
                masterConn.Close();
            }

            var connString = $"Server=(localdb)\\MSSQLLocalDB;Integrated Security=true;Initial Catalog={dbName}";

            using var conn = new SqlConnection(connString);
            conn.Open();

            // create tables first
            conn.Execute(@"
                CREATE TABLE Books (
                    BookId INT IDENTITY(1,1) PRIMARY KEY,
                    BookName NVARCHAR(200) NOT NULL
                );

                CREATE TABLE Borrowers (
                    BorrowerId INT IDENTITY(1,1) PRIMARY KEY,
                    Name NVARCHAR(200) NOT NULL
                );

                CREATE TABLE Loans (
                    LoanId INT IDENTITY(1,1) PRIMARY KEY,
                    BookId INT NOT NULL,
                    BorrowerId INT NOT NULL,
                    BorrowDate DATETIME2 NOT NULL,
                    ReturnDate DATETIME2 NULL
                );
            ");

            // create stored procedures in separate batches
            conn.Execute(@"
                CREATE PROCEDURE dbo.GetBooksByBorrowedCount
                AS
                BEGIN
                    SELECT b.BookId, b.BookName, ISNULL(COUNT(l.LoanId),0) AS TotalBorrowed
                    FROM Books b
                    LEFT JOIN Loans l ON b.BookId = l.BookId
                    GROUP BY b.BookId, b.BookName
                    ORDER BY TotalBorrowed DESC
                END
            ");

            conn.Execute(@"
                CREATE PROCEDURE dbo.GetUserLendingRate
                    @FromDate DATETIME2,
                    @ToDate DATETIME2
                AS
                BEGIN
                    SELECT br.BorrowerId, br.Name, COUNT(*) AS BorrowCount
                    FROM Loans l
                    INNER JOIN Borrowers br ON l.BorrowerId = br.BorrowerId
                    WHERE l.BorrowDate >= @FromDate AND l.BorrowDate <= @ToDate
                    GROUP BY br.BorrowerId, br.Name
                    ORDER BY BorrowCount DESC
                END
            ");

            conn.Execute(@"
                CREATE PROCEDURE dbo.GetUserReadingPace
                    @BorrowerId INT
                AS
                BEGIN
                    SELECT br.BorrowerId, b.BookName, l.ReturnDate, 
                        CASE WHEN l.ReturnDate IS NULL THEN 0 ELSE DATEDIFF(day, l.BorrowDate, l.ReturnDate) END AS ReadingPace
                    FROM Loans l
                    INNER JOIN Borrowers br ON l.BorrowerId = br.BorrowerId
                    INNER JOIN Books b ON l.BookId = b.BookId
                    WHERE br.BorrowerId = @BorrowerId
                END
            ");

            conn.Execute(@"
                CREATE PROCEDURE dbo.GetUserBorrowingPatterns
                    @BookId INT
                AS
                BEGIN
                    SELECT DISTINCT other.BookId, other.BookName
                    FROM Loans l
                    INNER JOIN Loans l2 ON l.BorrowerId = l2.BorrowerId
                    INNER JOIN Books other ON l2.BookId = other.BookId
                    WHERE l.BookId = @BookId AND other.BookId <> @BookId
                END
            ");

            return connString;
        }

        public static void DropTestDatabase(string dbName)
        {
            using var masterConn = new SqlConnection("Server=(localdb)\\MSSQLLocalDB;Integrated Security=true;Initial Catalog=master");
            masterConn.Open();
            try
            {
                masterConn.Execute($"IF DB_ID('{dbName}') IS NOT NULL BEGIN ALTER DATABASE [{dbName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [{dbName}]; END");
            }
            catch
            {
                // best effort cleanup
            }
        }
    }
}
