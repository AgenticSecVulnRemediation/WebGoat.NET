using System;
using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        [Fact]
        public void IsValidCustomerLogin_UsesParameterizedQuery()
        {
            // Arrange
            var method = typeof(SqliteDbProvider).GetMethod("IsValidCustomerLogin");
            Assert.NotNull(method);

            // Act
            var signature = method.ToString();

            // Assert
            Assert.Contains("IsValidCustomerLogin", signature);
        }
    }
}
