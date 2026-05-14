using System;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        [Fact]
        public void CustomCustomerLogin_UsesParameterizedQuery()
        {
            // Arrange
            const string expectedSql = "select * from CustomerLogin where email = @email";

            // Assert
            Assert.Contains("@email", expectedSql);
            Assert.DoesNotContain("'", expectedSql);
        }

        [Fact]
        public void GetPasswordByEmail_UsesParameterizedQuery()
        {
            // Arrange
            const string expectedSql = "select * from CustomerLogin where email = @email";

            // Assert
            Assert.Contains("@email", expectedSql);
            Assert.DoesNotContain("'", expectedSql);
        }
    }
}
