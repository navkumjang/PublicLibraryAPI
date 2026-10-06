using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using PublicLibrary.Service.Services;
using PublicLibrary.Domain.Interfaces;
using PublicLibrary.Domain.Entities;
using Google.Protobuf.WellKnownTypes;

namespace PublicLibrary.UnitTests.Services
{
    public class BooksInsightsServiceTests
    {
        [Test]
        public async Task GetInventoryInsights_WhenRepositoryReturnsInsights_MapsResponseCorrectly()
        {
            // Arrange
            var repoMock = new Mock<IBookRepository>();
            repoMock.Setup(r => r.GetInventoryInsightsAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<InventoryInsight>
                {
                    new InventoryInsight { BookId = 1, BookName = "A", TotalBorrowed = 10 },
                    new InventoryInsight { BookId = 2, BookName = "B", TotalBorrowed = 5 }
                });

            var loggerMock = new Mock<Microsoft.Extensions.Logging.ILogger<BooksInsightsService>>();

            var svc = new BooksInsightsService(repoMock.Object, loggerMock.Object);

            // Act
            var resp = await svc.GetInventoryInsights(new Empty(), PublicLibrary.UnitTests.Support.TestServerCallContext.Create());

            // Assert
            Assert.That(resp, Is.Not.Null);
            Assert.That(resp.Insights, Has.Count.EqualTo(2));
            var first = resp.Insights.First();
            Assert.That(first.BookId, Is.EqualTo(1));
            Assert.That(first.BookName, Is.EqualTo("A"));
            Assert.That(first.TotalBorrowed, Is.EqualTo(10));
        }
    }
}
