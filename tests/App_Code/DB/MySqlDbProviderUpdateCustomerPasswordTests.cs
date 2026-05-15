using System;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderUpdateCustomerPasswordTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameterizedQueryText()
        {
            // Arrange
            // We avoid DB dependency; this delta test ensures the SQL text was changed to parameterized placeholders.
            var asmText = System.IO.File.ReadAllText(typeof(MySqlDbProvider).Assembly.Location);

            // Assert
            Assert.Contains("UPDATE CustomerLogin SET password = @password WHERE customerNumber = @customerNumber", asmText);
            Assert.DoesNotContain("update CustomerLogin set password = '\" +", asmText);
        }
    }
}
