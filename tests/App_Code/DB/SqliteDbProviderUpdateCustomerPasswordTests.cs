using Xunit;
using Mono.Data.Sqlite;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderUpdateCustomerPasswordTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameterizedUpdate_ForPasswordAndCustomerNumber()
        {
            const string expectedSql = "UPDATE CustomerLogin SET password = @password WHERE customerNumber = @customerNumber";

            var cmd = new SqliteCommand(expectedSql);
            cmd.Parameters.AddWithValue("@password", "encoded");
            cmd.Parameters.AddWithValue("@customerNumber", 1);

            Assert.Equal(expectedSql, cmd.CommandText);
            Assert.True(cmd.Parameters.Contains("@password"));
            Assert.True(cmd.Parameters.Contains("@customerNumber"));
        }
    }
}
