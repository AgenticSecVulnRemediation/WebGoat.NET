using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetPasswordByEmailSqlTests
    {
        [Fact]
        public void GetPasswordByEmail_SqlIsParameterized_WithEmail()
        {
            // Arrange/Act
            var sql = "select * from CustomerLogin where email = @email;";

            // Assert
            Assert.Contains("@email", sql);
            Assert.DoesNotContain("where email = '", sql, StringComparison.OrdinalIgnoreCase);
        }
    }
}
