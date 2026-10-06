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
    public class UserRepositoryTests
    {
        private string _connString = string.Empty;
        private string _dbName = string.Empty;

        [SetUp]
        public void SetUp()
        {
            _dbName = "PublicLibraryTests_Db" + Guid.NewGuid().ToString("N");
            _connString = PublicLibrary.IntegrationTests.Support.TestDatabase.CreateTestDatabase(_dbName);

            using var conn = new SqlConnection(_connString);
            conn.Open();

            conn.Execute("INSERT INTO Books (BookName) VALUES (@n)", new[] { new { n = "BookA" }, new { n = "BookB" } });
            conn.Execute("INSERT INTO Borrowers (Name) VALUES (@n)", new[] { new { n = "Alice" }, new { n = "Bob" } });

            // loans across dates
            var now = DateTime.UtcNow.Date;
            conn.Execute("INSERT INTO Loans (BookId, BorrowerId, BorrowDate, ReturnDate) VALUES (1,1, @d1, @r1)", new { d1 = now.AddDays(-10), r1 = now.AddDays(-7) });
            conn.Execute("INSERT INTO Loans (BookId, BorrowerId, BorrowDate, ReturnDate) VALUES (2,1, @d2, @r2)", new { d2 = now.AddDays(-5), r2 = now.AddDays(-3) });
            conn.Execute("INSERT INTO Loans (BookId, BorrowerId, BorrowDate, ReturnDate) VALUES (1,2, @d3, @r3)", new { d3 = now.AddDays(-2), r3 = now.AddDays(-1) });
        }

        [TearDown]
        public void TearDown()
        {
            PublicLibrary.IntegrationTests.Support.TestDatabase.DropTestDatabase(_dbName);
        }

        [Test]
        public void GetUserLendingRate_FiltersByDateRange()
        {
            var factory = new TestConnectionFactory(_connString);
            var repo = new PublicLibrary.Infrastructure.Repositories.UserRepository(factory);

            var from = DateTime.UtcNow.Date.AddDays(-6);
            var to = DateTime.UtcNow.Date;

            var result = repo.GetUserLendingRateAsync(from, to).GetAwaiter().GetResult().ToList();

            Assert.That(result.Count, Is.EqualTo(2));
            var alice = result.FirstOrDefault(x => x.Name == "Alice");
            var bob = result.FirstOrDefault(x => x.Name == "Bob");
            Assert.That(alice.BorrowCount, Is.EqualTo(1));
            Assert.That(bob.BorrowCount, Is.EqualTo(1));
        }

        [Test]
        public void GetUserReadingPace_ReturnsReadingPace()
        {
            var factory = new TestConnectionFactory(_connString);
            var repo = new PublicLibrary.Infrastructure.Repositories.UserRepository(factory);

            var result = repo.GetUserReadingPaceAsync(1).GetAwaiter().GetResult().ToList();

            Assert.That(result.Count, Is.GreaterThan(0));
            var entry = result.First();
            Assert.That(entry.BorrowerId, Is.EqualTo(1));
            Assert.That(entry.BookName, Is.Not.Empty);
            Assert.That(entry.ReadingPace, Is.GreaterThanOrEqualTo(0));
        }

        [Test]
        public void GetUserBorrowingPatterns_ReturnsRelatedBooks()
        {
            var factory = new TestConnectionFactory(_connString);
            var repo = new PublicLibrary.Infrastructure.Repositories.UserRepository(factory);

            var result = repo.GetUserBorrowingPatternsAsync(1).GetAwaiter().GetResult().ToList();

            // Borrowers who borrowed book 1 also borrowed book 2
            Assert.That(result.Any(r => r.BookName == "BookB"));
        }
    }

    // uses Support/TestConnectionFactory
}
