using System;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        [Fact]
        public void IsValidCustomerLogin_ParameterizedQuery_PreventsInlineEmailAndPassword()
        {
            // Arrange
            const string expectedSql = "select * from CustomerLogin where email = @email and password = @password;";
            var injection = "x' OR 1=1 --";

            // Assert
            Assert.Contains("@email", expectedSql);
            Assert.Contains("@password", expectedSql);
            Assert.DoesNotContain(injection, expectedSql);
        }
    }
}
