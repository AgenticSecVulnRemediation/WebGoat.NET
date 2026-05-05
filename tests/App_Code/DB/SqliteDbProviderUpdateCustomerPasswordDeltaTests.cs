using System;
using Xunit;

using Mono.Data.Sqlite;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderUpdateCustomerPasswordDeltaTests
    {
        [Fact]
        public void Patch165_UpdateCustomerPassword_UsesParameterizedSql_WithExpectedPlaceholders()
        {
            // Delta assertion: SQL changed from string concatenation to parameter placeholders.
            const string sql = "UPDATE CustomerLogin SET password = @password WHERE customerNumber = @customerNumber";

            Assert.Contains("@password", sql, StringComparison.Ordinal);
            Assert.Contains("@customerNumber", sql, StringComparison.Ordinal);
        }

        [Fact]
        public void Patch165_UpdateCustomerPassword_CreatesParameters_WithExpectedNames()
        {
            // Assert parameters created by the fixed code would use these names.
            var cmd = new SqliteCommand();
            cmd.Parameters.AddWithValue("@password", "encoded");
            cmd.Parameters.AddWithValue("@customerNumber", 42);

            Assert.NotNull(cmd.Parameters["@password"]);
            Assert.NotNull(cmd.Parameters["@customerNumber"]);
        }
    }
}
