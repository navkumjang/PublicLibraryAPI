using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using PublicLibrary.Service.Services;
using PublicLibrary.Domain.Interfaces;
using PublicLibrary.Domain.Entities;
using PublicLibrary.Contracts.Protos;
using Grpc.Core;

namespace PublicLibrary.UnitTests.Services
{
    public class UserInsightsServiceTests
    {
        [Test]
        public async Task GetUserLendingRate_WhenRepositoryReturnsUsers_MapsResponseCorrectly()
        {
            // Arrange
            var repoMock = new Mock<IUserRepository>();
            repoMock.Setup(r => r.GetUserLendingRateAsync(It.IsAny<System.DateTime>(), It.IsAny<System.DateTime>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new System.Collections.Generic.List<PublicLibrary.Domain.Entities.UserLendingRate>
                {
                    new PublicLibrary.Domain.Entities.UserLendingRate { BorrowerId = 1, Name = "John", BorrowCount = 3 }
                });

            var loggerMock = new Mock<Microsoft.Extensions.Logging.ILogger<UserInsightsService>>();

            var svc = new UserInsightsService(repoMock.Object, loggerMock.Object);

            var req = new UserLendingRateRequest
            {
                FromDate = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(System.DateTime.UtcNow.AddDays(-7)),
                ToDate = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(System.DateTime.UtcNow)
            };

            // Act
            var resp = await svc.GetUserLendingRate(req, PublicLibrary.UnitTests.Support.TestServerCallContext.Create());

            // Assert
            Assert.That(resp, Is.Not.Null);
            Assert.That(resp.Users, Has.Count.EqualTo(1));
            var u = resp.Users.First();
            Assert.That(u.BorrowerId, Is.EqualTo(1));
            Assert.That(u.Name, Is.EqualTo("John"));
            Assert.That(u.BorrowCount, Is.EqualTo(3));
        }

        [Test]
        public async Task GetUserReadingPace_WhenRepositoryReturnsPace_MapsResponseCorrectly()
        {
            var repoMock = new Mock<IUserRepository>();
            repoMock.Setup(r => r.GetUserReadingPaceAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new System.Collections.Generic.List<PublicLibrary.Domain.Entities.UserReadingPace>
                {
                    new PublicLibrary.Domain.Entities.UserReadingPace { BorrowerId = 1, BookName = "Book1", ReturnDate = System.DateTime.UtcNow, ReadingPace = 5.5 }
                });

            var loggerMock = new Mock<Microsoft.Extensions.Logging.ILogger<UserInsightsService>>();
            var svc = new UserInsightsService(repoMock.Object, loggerMock.Object);

            var req = new UserReadingPaceRequest { BorrowId = 1 };

            var resp = await svc.GetUserReadingPace(req, PublicLibrary.UnitTests.Support.TestServerCallContext.Create());

            Assert.That(resp, Is.Not.Null);
            Assert.That(resp.Pace, Has.Count.EqualTo(1));
            var p = resp.Pace.First();
            Assert.That(p.BorrowerId, Is.EqualTo(1));
            Assert.That(p.BookName, Is.EqualTo("Book1"));
            Assert.That(p.ReadingPace, Is.EqualTo(5.5).Within(0.0001));
            Assert.That(p.ReturnDate, Is.Not.Null);
        }

        [Test]
        public async Task GetUserBorrowingPatterns_WhenRepositoryReturnsPatterns_MapsResponseCorrectly()
        {
            var repoMock = new Mock<IUserRepository>();
            repoMock.Setup(r => r.GetUserBorrowingPatternsAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new System.Collections.Generic.List<PublicLibrary.Domain.Entities.UserBorrowingPattern>
                {
                    new PublicLibrary.Domain.Entities.UserBorrowingPattern { BookId = 2, BookName = "OtherBook" }
                });

            var loggerMock = new Mock<Microsoft.Extensions.Logging.ILogger<UserInsightsService>>();
            var svc = new UserInsightsService(repoMock.Object, loggerMock.Object);

            var req = new UserBorrowingPatternsRequest { BookId = 1 };

            var resp = await svc.GetUserBorrowingPatterns(req, PublicLibrary.UnitTests.Support.TestServerCallContext.Create());

            Assert.That(resp, Is.Not.Null);
            Assert.That(resp.Patterns, Has.Count.EqualTo(1));
            var p = resp.Patterns.First();
            Assert.That(p.BookId, Is.EqualTo(2));
            Assert.That(p.BookName, Is.EqualTo("OtherBook"));
        }
    }
}
