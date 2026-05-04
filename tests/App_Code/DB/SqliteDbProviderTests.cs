using System;
using Mono.Data.Sqlite;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        [Fact]
        public void IsValidCustomerLogin_UsesParameterizedQuery_IncludesEmailAndPasswordParameters()
        {
            // Arrange
            const string expectedSql = "select * from CustomerLogin where email = @email and password = @password;";

            // Assert
            Assert.Contains("@email", expectedSql);
            Assert.Contains("@password", expectedSql);
            Assert.DoesNotContain("email = '", expectedSql, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("password = '", expectedSql, StringComparison.OrdinalIgnoreCase);
        }
    }
}
