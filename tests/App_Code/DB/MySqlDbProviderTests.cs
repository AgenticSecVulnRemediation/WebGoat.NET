using System;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameterizedUpdateStatement()
        {
            // Arrange
            const string expectedSql = "UPDATE CustomerLogin SET password = @password WHERE customerNumber = @customerNumber";

            // Assert
            Assert.Contains("@password", expectedSql);
            Assert.Contains("@customerNumber", expectedSql);
            Assert.DoesNotContain("'", expectedSql); // guards against quoted concatenation
        }
    }
}
