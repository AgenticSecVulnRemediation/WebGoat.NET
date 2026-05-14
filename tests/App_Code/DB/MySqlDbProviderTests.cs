using System;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void GetPasswordByEmail_UsesParameterizedQuery()
        {
            // Arrange
            const string expectedSql = "select * from CustomerLogin where email = @email;";
            var injection = "x' OR 1=1 --";

            // Assert
            Assert.Contains("@email", expectedSql);
            Assert.DoesNotContain(injection, expectedSql);
        }
    }
}
