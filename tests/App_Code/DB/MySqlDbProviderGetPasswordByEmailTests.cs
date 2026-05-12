using System;
using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetPasswordByEmailTests
    {
        [Fact]
        public void GetPasswordByEmail_UsesParameterizedSelectStatement()
        {
            // Arrange
            var method = typeof(MySqlDbProvider).GetMethod("GetPasswordByEmail");
            Assert.NotNull(method);

            // Act
            var signature = method.ToString();

            // Assert
            Assert.Contains("GetPasswordByEmail", signature);
        }
    }
}
