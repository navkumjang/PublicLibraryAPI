using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using PublicLibrary.SystemTests.Support;
using Dapper;
using Microsoft.Data.SqlClient;
using PublicLibrary.API.GrpcClients;
using PublicLibrary.Contracts.Protos;
using Microsoft.Extensions.Hosting;

namespace PublicLibrary.SystemTests.FullSystem
{
    public class EndToEndTests
    {
        private string _dbName = string.Empty;
        private string _connString = string.Empty;

        [SetUp]
        public void SetUp()
        {
            _dbName = "PublicLibrarySystem_Db" + Guid.NewGuid().ToString("N");
            _connString = TestDatabase.CreateTestDatabase(_dbName);

            // seed data
            using var conn = new SqlConnection(_connString);
            conn.Open();
            conn.Execute("INSERT INTO Books (BookName) VALUES (@n)", new[] { new { n = "BookA" }, new { n = "BookB" } });
            conn.Execute("INSERT INTO Borrowers (Name) VALUES (@n)", new[] { new { n = "Alice" }, new { n = "Bob" } });
            conn.Execute("INSERT INTO Loans (BookId, BorrowerId, BorrowDate, ReturnDate) VALUES (1,1, @d, @r)", new { d = DateTime.UtcNow.AddDays(-7), r = DateTime.UtcNow.AddDays(-4) });
            conn.Execute("INSERT INTO Loans (BookId, BorrowerId, BorrowDate, ReturnDate) VALUES (2,1, @d2, @r2)", new { d2 = DateTime.UtcNow.AddDays(-3), r2 = DateTime.UtcNow.AddDays(-1) });
        }

        [TearDown]
        public void TearDown()
        {
            TestDatabase.DropTestDatabase(_dbName);
        }

        [Test]
        public async Task GetInventoryInsights_FullStack_ReturnsCounts()
        {
            // start gRPC service with overridden DB connection factory
            var serviceFactory = new WebApplicationFactory<PublicLibrary.Service.TestEntryPoint>().WithWebHostBuilder(builder =>
            {
                builder.UseSetting("https_port", "0");
                builder.ConfigureServices(services =>
                {
                    // ensure controllers use Newtonsoft in the test host to avoid System.Text.Json PipeWriter issue
                    services.AddControllers().AddNewtonsoftJson();

                    services.AddSingleton<PublicLibrary.Infrastructure.Data.IDbConnectionFactory>(new TestConnectionFactory(_connString));
                });
            });

            // create channel to gRPC service
            var serviceClient = serviceFactory.CreateClient();
            var channel = GrpcChannel.ForAddress(serviceClient.BaseAddress, new GrpcChannelOptions { HttpClient = serviceClient });
            var bookGrpc = new PublicLibrary.Contracts.Protos.BookService.BookServiceClient(channel);

            // start API with gRPC clients wired to service channel
            var apiFactory = new WebApplicationFactory<PublicLibraryAPI.TestEntryPoint>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // ensure controllers use Newtonsoft in the test host to avoid System.Text.Json PipeWriter issue
                    services.AddControllers().AddNewtonsoftJson();

                    // replace IBookGrpcClient with one that uses the channel created above
                    services.AddSingleton<IBookGrpcClient>(new PublicLibrary.API.GrpcClients.BookGrpcClient(bookGrpc));
                });
            });

            var client = apiFactory.CreateClient();

            var res = await client.GetAsync("/Book/GetInventoryInsights");
            Assert.That(res.IsSuccessStatusCode, Is.True);

            var arr = await res.Content.ReadFromJsonAsync<System.Collections.Generic.List<PublicLibrary.API.Models.InventoryInsightDto>>();
            Assert.That(arr, Is.Not.Null);
            Assert.That(arr.Count, Is.GreaterThan(0));
        }

        [Test]
        public async Task GetUserReadingPace_FullStack_ReturnsPace()
        {
            var serviceFactory = new WebApplicationFactory<PublicLibrary.Service.TestEntryPoint>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddControllers().AddNewtonsoftJson();
                    services.AddSingleton<PublicLibrary.Infrastructure.Data.IDbConnectionFactory>(new TestConnectionFactory(_connString));
                });
            });

            var serviceClient = serviceFactory.CreateClient();
            var channel = GrpcChannel.ForAddress(serviceClient.BaseAddress, new GrpcChannelOptions { HttpClient = serviceClient });
            var userGrpc = new PublicLibrary.Contracts.Protos.UserService.UserServiceClient(channel);

            var apiFactory = new WebApplicationFactory<PublicLibraryAPI.TestEntryPoint>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddControllers().AddNewtonsoftJson();
                    services.AddSingleton<IUserGrpcClient>(new PublicLibrary.API.GrpcClients.UserGrpcClient(userGrpc));
                });
            });

            var client = apiFactory.CreateClient();
            var res = await client.GetAsync("/User/GetUserReadingPace?borrowerId=1");
            Assert.That(res.IsSuccessStatusCode, Is.True);

            var arr = await res.Content.ReadFromJsonAsync<System.Collections.Generic.List<PublicLibrary.API.Models.UserReadingPaceDto>>();
            Assert.That(arr, Is.Not.Null);
            Assert.That(arr.Count, Is.GreaterThan(0));
        }
    }

    internal class TestConnectionFactory : PublicLibrary.Infrastructure.Data.IDbConnectionFactory
    {
        private readonly string _cs;
        public TestConnectionFactory(string cs) { _cs = cs; }
        public global::System.Data.IDbConnection CreateConnection() => new Microsoft.Data.SqlClient.SqlConnection(_cs);
    }
}
