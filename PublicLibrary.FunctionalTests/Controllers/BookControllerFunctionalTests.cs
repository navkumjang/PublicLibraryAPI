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
    public class BookControllerFunctionalTests
    {
        [Test]
        public async Task GetInventoryInsights_ReturnsExpectedJson()
        {
            var factory = new WebApplicationFactory<PublicLibrary.API.Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Use NewtonsoftJson for test host to avoid System.Text.Json PipeWriter issue
                    services.AddControllers().AddNewtonsoftJson();

                    // replace IBookGrpcClient with mock
                    var mock = new Mock<IBookGrpcClient>();
                    var resp = new InventoryInsightsResponse();
                    resp.Insights.Add(new InventoryInsight { BookId = 1, BookName = "BookA", TotalBorrowed = 3 });
                    resp.Insights.Add(new InventoryInsight { BookId = 2, BookName = "BookB", TotalBorrowed = 1 });
                    mock.Setup(m => m.GetInventoryInsightsAsync(It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync(resp);

                    services.AddSingleton(mock.Object);
                });
            });

            var client = factory.CreateClient();

            var res = await client.GetAsync("/Book/GetInventoryInsights");
            Assert.That(res.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var arr = await res.Content.ReadFromJsonAsync<List<PublicLibrary.API.Models.InventoryInsightDto>>();
            Assert.That(arr, Is.Not.Null);
            Assert.That(arr.Count, Is.EqualTo(2));
            Assert.That(arr.First().BookName, Is.EqualTo("BookA"));
            Assert.That(arr.First().TotalBorrowed, Is.EqualTo(3));
        }
    }
}
