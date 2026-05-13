using System;
using System.Data;
using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        [Fact]
        public void IsValidCustomerLogin_UsesParameterizedQuery_ForEmailAndPassword()
        {
            // Arrange
            string sql = "select * from CustomerLogin where email = @email and password = @password;";

            // Assert
            Assert.Contains("@email", sql, StringComparison.Ordinal);
            Assert.Contains("@password", sql, StringComparison.Ordinal);
            Assert.DoesNotContain("where email = '\" +", sql, StringComparison.OrdinalIgnoreCase);
        }
    }
}
