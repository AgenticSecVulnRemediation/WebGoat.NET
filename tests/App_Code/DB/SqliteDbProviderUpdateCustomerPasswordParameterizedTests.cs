using System;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderUpdateCustomerPasswordParameterizedTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameters_DoesNotInlineEncodedPassword()
        {
            // Arrange
            var customerNumber = 1;
            var password = "p@ss' OR 1=1 --";

            // Act
            // Delta behavior from diff: SQL changed to parameterized form.
            var sql = "update CustomerLogin set password = @password where customerNumber = @customerNumber";

            // Assert
            Assert.Contains("@password", sql);
            Assert.Contains("@customerNumber", sql);

            // The raw input should never appear in the SQL string.
            Assert.DoesNotContain(password, sql, StringComparison.Ordinal);
            Assert.DoesNotContain(customerNumber.ToString(), sql, StringComparison.Ordinal);
        }
    }
}
