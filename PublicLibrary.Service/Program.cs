using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PublicLibrary.Service.Services;
using PublicLibrary.Infrastructure.Repositories;
using PublicLibrary.Domain.Interfaces;
using PublicLibrary.Service;

namespace PublicLibrary.Service;

public class Program
{
    public static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddGrpc();
        // Register infrastructure implementations
        builder.Services.AddSingleton<PublicLibrary.Infrastructure.Data.IDbConnectionFactory, PublicLibrary.Infrastructure.Data.SqlConnectionFactory>();
        builder.Services.AddScoped<IBookRepository, BookRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.

        app.MapGrpcService<BooksInsightsService>();
        app.MapGrpcService<UserInsightsService>();
        app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

        app.Run();
    }
}

