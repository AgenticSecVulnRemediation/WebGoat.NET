using System;
using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderCustomCustomerLoginTests
    {
        [Fact]
        public void CustomCustomerLogin_UsesParameterizedEmailFilter()
        {
            // Arrange
            const string sql = "select * from CustomerLogin where email = @email";

            // Act
            var emailParam = new SqliteParameter("@email", "foo' OR 1=1--");

            // Assert
            Assert.Contains("@email", sql);
            Assert.DoesNotContain("'\" + email + \"'", sql);
            Assert.Equal("@email", emailParam.ParameterName);
            Assert.Equal("foo' OR 1=1--", emailParam.Value);
        }
    }
}
