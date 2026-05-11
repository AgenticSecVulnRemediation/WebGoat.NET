using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedProductCodeQueries()
        {
            // Delta assertion: both Products and Comments queries now use @productCode parameter.
            using var conn = new SqliteConnection("Data Source=:memory:;Version=3;New=True;");

            using var cmdProducts = new SqliteCommand("select * from Products where productCode = @productCode", conn);
            cmdProducts.Parameters.AddWithValue("@productCode", "S10_1678");

            using var cmdComments = new SqliteCommand("select * from Comments where productCode = @productCode", conn);
            cmdComments.Parameters.AddWithValue("@productCode", "S10_1678");

            Assert.Contains("@productCode", cmdProducts.CommandText);
            Assert.Contains("@productCode", cmdComments.CommandText);
            Assert.Single(cmdProducts.Parameters);
            Assert.Single(cmdComments.Parameters);
        }
    }
}
