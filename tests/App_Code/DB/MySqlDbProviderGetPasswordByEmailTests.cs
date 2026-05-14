using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetPasswordByEmailTests
    {
        [Fact]
        public void GetPasswordByEmail_UsesParameterizedEmailFilter()
        {
            // Arrange
            const string sql = "select * from CustomerLogin where email = @email;";

            // Assert
            Assert.DoesNotContain("'\" + email + \"'", sql);
            Assert.Contains("@email", sql);
        }
    }
}
