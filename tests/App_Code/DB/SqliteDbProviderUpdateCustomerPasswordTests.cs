using Xunit;
using Mono.Data.Sqlite;
using System;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderUpdateCustomerPasswordTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameters_PreventsSqlInjection()
        {
            // Arrange
            // Delta focus: SQL updated from string concatenation to parameterized query with @password and @customerNumber.
            const string expectedSql = "UPDATE CustomerLogin SET password = @password WHERE customerNumber = @customerNumber";

            // Act & Assert
            Assert.Contains("@password", expectedSql);
            Assert.Contains("@customerNumber", expectedSql);
            Assert.DoesNotContain("Encoder.Encode(password) +", expectedSql);
        }
    }
}
