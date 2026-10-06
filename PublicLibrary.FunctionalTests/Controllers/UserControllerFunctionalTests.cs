using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using NUnit.Framework;
using PublicLibrary.API.GrpcClients;
using PublicLibrary.Contracts.Protos;
using System.Collections.Generic;
using Google.Protobuf.WellKnownTypes;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace PublicLibrary.FunctionalTests.Controllers
{
    public class UserControllerFunctionalTests
    {
        [Test]
        public async Task GetUserLendingRate_ReturnsExpectedJson()
        {
            var factory = new WebApplicationFactory<PublicLibrary.API.Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddControllers().AddNewtonsoftJson();

                    var mock = new Mock<IUserGrpcClient>();
                    var resp = new UserLendingRateResponse();
                    resp.Users.Add(new UserLendingRate { BorrowerId = 1, Name = "Alice", BorrowCount = 2 });
                    mock.Setup(m => m.GetUserLendingRateAsync(It.IsAny<System.DateTime>(), It.IsAny<System.DateTime>(), It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync(resp);

                    services.AddSingleton<IUserGrpcClient>(mock.Object);
                });
            });

            var client = factory.CreateClient();

            var from = System.DateTime.UtcNow.AddDays(-10).ToString("o");
            var to = System.DateTime.UtcNow.ToString("o");

            var res = await client.GetAsync($"/User/GetUserLendingRate?fromDate={System.Net.WebUtility.UrlEncode(from)}&toDate={System.Net.WebUtility.UrlEncode(to)}");
            Assert.That(res.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var arr = await res.Content.ReadFromJsonAsync<List<PublicLibrary.API.Models.UserLendingRateDto>>();
            Assert.That(arr, Is.Not.Null);
            Assert.That(arr.Count, Is.EqualTo(1));
            Assert.That(arr.First().Name, Is.EqualTo("Alice"));
            Assert.That(arr.First().BorrowCount, Is.EqualTo(2));
        }

        [Test]
        public async Task GetUserReadingPace_ReturnsExpectedWhenPresent()
        {
            var factory = new WebApplicationFactory<PublicLibrary.API.Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddControllers().AddNewtonsoftJson();

                    var mock = new Mock<IUserGrpcClient>();
                    var resp = new UserReadingPaceResponse();
                    resp.Pace.Add(new UserReadingPace { BorrowerId = 1, BookName = "Book1", ReadingPace = 3.0, ReturnDate = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.UtcNow) });
                    mock.Setup(m => m.GetUserReadingPaceAsync(It.IsAny<int>(), It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync(resp);
                    services.AddSingleton<IUserGrpcClient>(mock.Object);
                });
            });

            var client = factory.CreateClient();
            var res = await client.GetAsync($"/User/GetUserReadingPace?borrowerId=1");
            Assert.That(res.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var arr = await res.Content.ReadFromJsonAsync<System.Collections.Generic.List<PublicLibrary.API.Models.UserReadingPaceDto>>();
            Assert.That(arr, Is.Not.Null);
            Assert.That(arr.Count, Is.EqualTo(1));
            Assert.That(arr.First().BookName, Is.EqualTo("Book1"));
        }

        [Test]
        public async Task GetUserBorrowingPatterns_ReturnsExpected()
        {
            var factory = new WebApplicationFactory<PublicLibrary.API.Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddControllers().AddNewtonsoftJson();

                    var mock = new Mock<IUserGrpcClient>();
                    var resp = new UserBorrowingPatternsResponse();
                    resp.Patterns.Add(new UserBorrowingPattern { BookId = 2, BookName = "Related" });
                    mock.Setup(m => m.GetUserBorrowingPatternsAsync(It.IsAny<int>(), It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync(resp);
                    services.AddSingleton<IUserGrpcClient>(mock.Object);
                });
            });

            var client = factory.CreateClient();
            var res = await client.GetAsync($"/User/GetUserBorrowingPatterns?bookId=1");
            Assert.That(res.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var arr = await res.Content.ReadFromJsonAsync<List<PublicLibrary.API.Models.UserBorrowingPatternDto>>();
            Assert.That(arr, Is.Not.Null);
            Assert.That(arr.First().BookName, Is.EqualTo("Related"));
        }
    }
}
