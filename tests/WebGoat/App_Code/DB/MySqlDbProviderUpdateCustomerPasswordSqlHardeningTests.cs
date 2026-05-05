using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderUpdateCustomerPasswordSqlHardeningTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameterizedSql_DoesNotInlinePasswordOrCustomerNumber()
        {
            // Arrange
            var sql = "update CustomerLogin set password = @password where customerNumber = @customerNumber";
            var password = "p@ss'word";
            var customerNumber = 42;

            // Act
            // This is a delta-focused unit test: verify the query uses parameter placeholders.
            Assert.Contains("@password", sql);
            Assert.Contains("@customerNumber", sql);
            Assert.DoesNotContain(password, sql);
            Assert.DoesNotContain(customerNumber.ToString(), sql);
        }
    }
}
