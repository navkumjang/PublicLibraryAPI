-- =============================================
-- Public Library Database Schema
-- SQL Server
-- =============================================

-- Create database if it does not already exist
IF DB_ID('PublicLibrary') IS NULL
BEGIN
    CREATE DATABASE PublicLibrary;
END
GO

USE PublicLibrary;
GO


-- =============================================
-- 1. Book
-- =============================================

IF OBJECT_ID('dbo.Book', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Book
    (
        Id              INT IDENTITY(1,1) NOT NULL,
        Name            NVARCHAR(200) NOT NULL,
        Author          NVARCHAR(200) NOT NULL,
        TotalPages      INT NOT NULL,
        Price           DECIMAL(10,2) NOT NULL,
        TotalCount      INT NOT NULL,
        TotalBorrowed   INT NOT NULL,
        IsActive        BIT NOT NULL
                        CONSTRAINT DF_Book_IsActive DEFAULT (1),
        IsDeleted       BIT NOT NULL
                        CONSTRAINT DF_Book_IsDeleted DEFAULT (0),

        CONSTRAINT PK_Book PRIMARY KEY (Id),

        CONSTRAINT CK_Book_TotalPages
            CHECK (TotalPages > 0),

        CONSTRAINT CK_Book_Price
            CHECK (Price >= 0),

        CONSTRAINT CK_Book_TotalCount
            CHECK (TotalCount >= 0),

        CONSTRAINT CK_Book_TotalBorrowed
            CHECK (
                TotalBorrowed >= 0
                AND TotalBorrowed <= TotalCount
            )
    );
END
GO


-- =============================================
-- 2. Borrower
-- =============================================

IF OBJECT_ID('dbo.Borrower', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Borrower
    (
        Id          INT IDENTITY(1,1) NOT NULL,
        Name        NVARCHAR(200) NOT NULL,
        IsActive    BIT NOT NULL
                    CONSTRAINT DF_Borrower_IsActive DEFAULT (1),
        IsDeleted   BIT NOT NULL
                    CONSTRAINT DF_Borrower_IsDeleted DEFAULT (0),

        CONSTRAINT PK_Borrower PRIMARY KEY (Id)
    );
END
GO


-- =============================================
-- 3. Lending
-- =============================================

IF OBJECT_ID('dbo.Lending', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Lending
    (
        Id          INT IDENTITY(1,1) NOT NULL,
        BorrowerId  INT NOT NULL,
        BookId      INT NOT NULL,
        FromDate    DATE NOT NULL,
        ToDate      DATE NULL,
        LendingDate DATETIME2(0) NOT NULL
                    CONSTRAINT DF_Lending_LendingDate
                    DEFAULT (SYSUTCDATETIME()),

        CONSTRAINT PK_Lending PRIMARY KEY (Id),

        CONSTRAINT FK_Lending_Borrower
            FOREIGN KEY (BorrowerId)
            REFERENCES dbo.Borrower(Id),

        CONSTRAINT FK_Lending_Book
            FOREIGN KEY (BookId)
            REFERENCES dbo.Book(Id),

        CONSTRAINT CK_Lending_Dates
            CHECK (ToDate IS NULL OR ToDate >= FromDate)
    );
END
GO