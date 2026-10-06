using System;
using System.Linq;
using Dapper;
using Microsoft.Data.SqlClient;
using NUnit.Framework;
using PublicLibrary.Infrastructure.Repositories;
using PublicLibrary.IntegrationTests.Support;

namespace PublicLibrary.IntegrationTests.Repositories
{
    [TestFixture]
    public class BookRepositoryTests
    {
        private string _connString = string.Empty;
        private string _dbName = string.Empty;

        [SetUp]
        public void SetUp()
        {
            _dbName = "PublicLibraryTests_Db" + Guid.NewGuid().ToString("N");
            _connString = TestDatabase.CreateTestDatabase(_dbName);

            using var conn = new SqlConnection(_connString);
            conn.Open();

            // seed data
            conn.Execute("INSERT INTO Books (BookName) VALUES (@n)", new[] { new { n = "BookA" }, new { n = "BookB" }, new { n = "BookC" } });
            conn.Execute("INSERT INTO Borrowers (Name) VALUES (@n)", new[] { new { n = "Alice" }, new { n = "Bob" } });

            // create loans: BookA x3, BookB x2, BookC x0
            conn.Execute("INSERT INTO Loans (BookId, BorrowerId, BorrowDate, ReturnDate) VALUES (1,1, @d, DATEADD(day,3,@d))", new { d = DateTime.UtcNow.AddDays(-10) });
            conn.Execute("INSERT INTO Loans (BookId, BorrowerId, BorrowDate, ReturnDate) VALUES (1,2, @d2, DATEADD(day,2,@d2))", new { d2 = DateTime.UtcNow.AddDays(-8) });
            conn.Execute("INSERT INTO Loans (BookId, BorrowerId, BorrowDate, ReturnDate) VALUES (1,1, @d3, DATEADD(day,1,@d3))", new { d3 = DateTime.UtcNow.AddDays(-5) });
            conn.Execute("INSERT INTO Loans (BookId, BorrowerId, BorrowDate, ReturnDate) VALUES (2,1, @d4, DATEADD(day,4,@d4))", new { d4 = DateTime.UtcNow.AddDays(-4) });
            conn.Execute("INSERT INTO Loans (BookId, BorrowerId, BorrowDate, ReturnDate) VALUES (2,2, @d5, DATEADD(day,2,@d5))", new { d5 = DateTime.UtcNow.AddDays(-2) });
        }

        [TearDown]
        public void TearDown()
        {
            TestDatabase.DropTestDatabase(_dbName);
        }

        [Test]
        public void GetInventoryInsights_ReturnsOrderedCounts()
        {
            var factory = new TestConnectionFactory(_connString);
            var repo = new BookRepository(factory);

            var list = repo.GetInventoryInsightsAsync().GetAwaiter().GetResult().ToList();

            Assert.That(list, Is.Not.Null);
            Assert.That(list.Count, Is.EqualTo(3));
            Assert.That(list[0].BookName, Is.EqualTo("BookA"));
            Assert.That(list[0].TotalBorrowed, Is.EqualTo(3));
            Assert.That(list[1].BookName, Is.EqualTo("BookB"));
            Assert.That(list[1].TotalBorrowed, Is.EqualTo(2));
            Assert.That(list[2].BookName, Is.EqualTo("BookC"));
            Assert.That(list[2].TotalBorrowed, Is.EqualTo(0));
        }
    }

    // uses Support/TestConnectionFactory
}
