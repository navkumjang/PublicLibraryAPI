namespace PublicLibrary.API;

public class Program
{
    public static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // read gRPC backend address from configuration (appsettings) and use for both services
        var grpcAddress = builder.Configuration["GrpcService:Address"] ?? "http://localhost:5268";

        // register generated gRPC clients using the client factory and configure address from config
        builder.Services.AddGrpcClient<PublicLibrary.Contracts.Protos.BookService.BookServiceClient>(opts =>
        {
            opts.Address = new Uri(grpcAddress);
        });

        // register user grpc client
        builder.Services.AddGrpcClient<PublicLibrary.Contracts.Protos.UserService.UserServiceClient>(opts =>
        {
            opts.Address = new Uri(grpcAddress);
        });

        // register wrappers that use the generated clients
        builder.Services.AddScoped<PublicLibrary.API.GrpcClients.IBookGrpcClient, PublicLibrary.API.GrpcClients.BookGrpcClient>();
        builder.Services.AddScoped<PublicLibrary.API.GrpcClients.IUserGrpcClient, PublicLibrary.API.GrpcClients.UserGrpcClient>();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
