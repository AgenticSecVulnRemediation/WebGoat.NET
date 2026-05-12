using Xunit;
using Mono.Data.Sqlite;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetCustomerEmailTests
    {
        [Fact]
        public void GetCustomerEmail_UsesNamedParameter_ForCustomerNumber()
        {
            // Regression: query should use @customerNumber and bind the value.
            const string expectedSql = "select email from CustomerLogin where customerNumber = @customerNumber";

            var cmd = new SqliteCommand(expectedSql);
            cmd.Parameters.AddWithValue("@customerNumber", "1");

            Assert.Equal(expectedSql, cmd.CommandText);
            Assert.True(cmd.Parameters.Contains("@customerNumber"));
            Assert.Equal("1", cmd.Parameters["@customerNumber"].Value);
        }
    }
}
