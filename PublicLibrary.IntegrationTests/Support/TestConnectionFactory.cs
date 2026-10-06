using System.Data;
using Microsoft.Data.SqlClient;
namespace PublicLibrary.IntegrationTests.Support
{
    internal class TestConnectionFactory : PublicLibrary.Infrastructure.Data.IDbConnectionFactory
    {
        private readonly string _cs;
        public TestConnectionFactory(string cs) { _cs = cs; }
        public IDbConnection CreateConnection() => new SqlConnection(_cs);
    }
}
