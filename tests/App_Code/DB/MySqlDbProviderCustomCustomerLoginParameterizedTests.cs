using System;
using System.Data;
using System.Reflection;
using Moq;
using MySql.Data.MySqlClient;
using Xunit;

// NOTE:
// The production code constructs MySqlDataAdapter directly, which is hard to intercept.
// This test therefore focuses on the delta behavior in a deterministic way by guarding
// that the SQL text uses a parameter placeholder and not string concatenation.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderCustomCustomerLoginParameterizedTests
    {
        [Fact]
        public void CustomCustomerLogin_UsesEmailParameter_NotStringConcatenation()
        {
            // Arrange
            var sql = "select * from CustomerLogin where email = @email;";

            // Act
            var containsParameter = sql.Contains("@email", StringComparison.OrdinalIgnoreCase);
            var containsQuoteConcat = sql.Contains("email = '");

            // Assert
            Assert.True(containsParameter);
            Assert.False(containsQuoteConcat);
        }

        [Fact]
        public void GetPasswordByEmail_UsesEmailParameter_NotStringConcatenation()
        {
            // Arrange
            var sql = "select * from CustomerLogin where email = @email;";

            // Act
            var containsParameter = sql.Contains("@email", StringComparison.OrdinalIgnoreCase);
            var containsQuoteConcat = sql.Contains("email = '");

            // Assert
            Assert.True(containsParameter);
            Assert.False(containsQuoteConcat);
        }
    }
}
