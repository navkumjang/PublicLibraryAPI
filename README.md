# PublicLibraryAPI — Developer README

Short guide to get this repository running locally and to run the automated test suite.

## Prerequisites
- .NET SDK 10 (the projects target net10.0)
- Visual Studio 2022 or a recent VS Code + C# extension
- Reachable SQL Server instance and SQL Server LocalDB (recommended for tests) 
- Git and a clone of this repository

## Project layout (high level)
- PublicLibraryAPI/         — REST API project
- PublicLibrary.Service/    — gRPC service project
- PublicLibrary.Infrastructure/ — Dapper repositories and DB support
- PublicLibrary.UnitTests/  — fast unit tests (Moq)
- PublicLibrary.IntegrationTests/ — integration tests using LocalDB
- PublicLibrary.FunctionalTests/  — functional HTTP tests using WebApplicationFactory
- PublicLibrary.SystemTests/      — full-stack system tests (HTTP → gRPC → DB)

## Setup the DB
Execute DB scripts (PublicLibraryAPI/SQLscripts) on you local SQL server instance in following order:
1. PublicLibrary-DBSchema.sql
2. PublicLibrary-StoredProc.sql
3. PublicLibrary-TestData.sql

## App settings you may need to update
- API gRPC target (PublicLibraryAPI/appsettings.json):
  - `GrpcService:Address` should point to the gRPC service host used in local development. 
	- You can run the service in Visual Studio, and it will print the listening address to the console. Copy the same to appSettings.json.

- Service SQL connection string (PublicLibrary.Service/appsettings.json):
  - `ConnectionStrings:Default` is used by the service to connect to your SQL Server instance. Set it as per your local SQL Server instance.
  - For tests, you can leave this unchanged because test projects create a temporary LocalDB instance, but when running the service manually you may need to point this at a real SQL Server or LocalDB instance. 
	- If using LocalDB the connection string typically looks like `Server=(localdb)\\MSSQLLocalDB;Database=YourDbName;Trusted_Connection=True;`.

## Quick start
1. Clone the repository.
2. From the repository root restore packages and build the solution.
3. Set both PublicLibrary.Service project and PublicLibrary.API as the startup projects and run them. (Connection strings must be set correctly in appsettings.json files.)
4. Get the API running and test it using Postman(with generated API localhost address) with below added Curls.
5. Run the full automated tests (unit, integration, functional, system).

## GetInventoryInsights
postman request 'https://localhost:7010/Book/GetInventoryInsights' \
  --header 'Accept: application/json'

## GetUserLendingRate
postman request 'https://localhost:7010/User/GetUserLendingRate?fromDate=2026-05-01T00%3A00%3A00Z&toDate=2026-08-31T23%3A59%3A59Z' \
  --header 'Accept: application/json'

## GetUserReadingPace
postman request 'https://localhost:7010/User/GetUserReadingPace?borrowerId=5' \
  --header 'Accept: application/json'

## GetUserBorrowingPatterns
postman request 'https://localhost:7010/User/GetUserBorrowingPatterns?bookId=5' \
  --header 'Accept: application/json'


