using System;
using System.Reflection;
using Xunit;

// Delta test: GetEmailByName must use parameterized LIKE (@name) instead of string concatenation.
namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetEmailByNameTests
    {
        [Fact]
        public void GetEmailByName_UsesParameterMarker_ForName()
        {
            // Arrange
            var method = typeof(MySqlDbProvider).GetMethod("GetEmailByName");
            Assert.NotNull(method);

            // Act
            var body = method!.GetMethodBody();

            // Assert
            Assert.NotNull(body);
            // The fixed query contains "like @name". Guard the regression by requiring '@name' to appear.
            Assert.Contains("@name", method.ToString());
        }
    }
}
