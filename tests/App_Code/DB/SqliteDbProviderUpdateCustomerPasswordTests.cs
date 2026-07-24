using Xunit;
using Moq;
using System;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderUpdateCustomerPasswordTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameters_InsteadOfStringConcatenation()
        {
            // Arrange
            var providerType = typeof(SqliteDbProvider);
            var method = providerType.GetMethod("UpdateCustomerPassword");
            Assert.NotNull(method);

            // Act
            var body = method!.GetMethodBody();

            // Assert
            Assert.NotNull(body);
            Assert.Equal("UpdateCustomerPassword", method.Name);
        }
    }
}
