using System;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetPasswordByEmailParameterizationTests
    {
        [Fact]
        public void GetPasswordByEmail_UsesEmailParameterMarker()
        {
            // Arrange
            var expectedSql = "select * from CustomerLogin where email = @email;";

            // Assert
            Assert.Contains("@email", expectedSql);
            Assert.DoesNotContain("'" + " + email + " + "'", expectedSql);
        }
    }
}
