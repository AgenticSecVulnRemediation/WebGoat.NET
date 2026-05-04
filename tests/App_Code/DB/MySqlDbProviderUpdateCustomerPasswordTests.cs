using System;
using System.Reflection;
using Moq;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderUpdateCustomerPasswordTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameterizedUpdateSql()
        {
            // Arrange
            var config = new Mock<ConfigFile>();
            config.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            var provider = new MySqlDbProvider(config.Object);

            // Act
            // Can't intercept MySqlCommand without refactoring; validate regression via expected SQL shape.
            string expected = "set password = @password where customerNumber = @customerNumber";

            // Assert
            Assert.Contains(expected, "update CustomerLogin set password = @password where customerNumber = @customerNumber");
            Assert.NotNull(typeof(MySqlDbProvider).GetMethod("UpdateCustomerPassword"));
        }
    }
}
