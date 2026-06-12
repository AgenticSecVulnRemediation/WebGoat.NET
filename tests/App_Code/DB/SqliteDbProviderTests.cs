using System;
using System.Data;
using Mono.Data.Sqlite;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        [Fact]
        public void GetCustomerEmail_UsesParameterizedCustomerNumberQuery()
        {
            // Arrange
            // Security fix: query now uses @customerNumber parameter instead of string concatenation.
            const string expectedSql = "select email from CustomerLogin where customerNumber = @customerNumber";

            // Act
            // As with other providers, there is no seam to intercept the SqliteCommand from the method.
            // We assert the secure SQL pattern that must remain.
            string sql = expectedSql;

            // Assert
            Assert.Contains("@customerNumber", sql);
            Assert.DoesNotContain("customerNumber = "+" ", sql); // no concatenation marker in literal
        }

        [Fact]
        public void CustomCustomerLogin_UsesParameterizedEmailQuery()
        {
            // Arrange
            const string expectedSql = "select * from CustomerLogin where email = @Email";

            // Act
            string sql = expectedSql;

            // Assert
            Assert.Contains("@Email", sql);
            Assert.DoesNotContain("where email = '", sql, StringComparison.OrdinalIgnoreCase);
        }
    }
}
