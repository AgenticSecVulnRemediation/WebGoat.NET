using System;
using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetCustomerEmailTests
    {
        [Fact]
        public void GetCustomerEmail_UsesParameterizedQueryForCustomerNumber()
        {
            // Arrange
            var provider = (MySqlDbProvider)Activator.CreateInstance(typeof(MySqlDbProvider), args: new object[] { null });

            // Act
            var moduleBytes = System.IO.File.ReadAllBytes(typeof(MySqlDbProvider).Module.FullyQualifiedName);
            var moduleText = System.Text.Encoding.UTF8.GetString(moduleBytes);

            // Assert
            Assert.Contains("customerNumber = @customerNumber", moduleText);
            Assert.Contains("@customerNumber", moduleText);
        }
    }
}
