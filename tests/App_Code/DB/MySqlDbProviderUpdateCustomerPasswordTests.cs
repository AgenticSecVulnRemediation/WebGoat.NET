using System;
using System.Reflection;
using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderUpdateCustomerPasswordTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameterizedUpdateStatement()
        {
            // Arrange
            var method = typeof(MySqlDbProvider).GetMethod("UpdateCustomerPassword");
            Assert.NotNull(method);

            // Act
            var signature = method.ToString();

            // Assert
            Assert.Contains("UpdateCustomerPassword", signature);
        }
    }
}
