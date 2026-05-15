using System;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderIsValidCustomerLoginTests
    {
        [Fact]
        public void IsValidCustomerLogin_UsesParametersForEmailAndPassword()
        {
            // Arrange
            var asmText = System.IO.File.ReadAllText(typeof(SqliteDbProvider).Assembly.Location);

            // Assert
            Assert.Contains("where email = @email and password = @password", asmText);
            Assert.Contains("command.Parameters.AddWithValue(\"@email\"", asmText);
            Assert.Contains("command.Parameters.AddWithValue(\"@password\"", asmText);
            Assert.DoesNotContain("where email = '\" + email", asmText);
        }
    }
}
