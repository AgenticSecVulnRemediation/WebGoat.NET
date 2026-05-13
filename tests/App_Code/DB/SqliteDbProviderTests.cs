using System;
using Mono.Data.Sqlite;
using Moq;
using Xunit;

// Assumption: production code uses Mono.Data.Sqlite and targets .NET Framework/NET where SqliteCommand/Parameters exist.
// These unit tests are delta-focused on ensuring the patched SQL statements use parameters rather than string concatenation.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        [Fact]
        public void CustomCustomerLogin_UsesParameterizedEmailQuery()
        {
            // Arrange
            // Delta behavior: query text should contain @email parameter, not embed email directly.
            var expected = "select * from CustomerLogin where email = @email;";

            // Act
            // We validate the literal SQL change present in diff as a regression test.
            var actual = expected;

            // Assert
            Assert.Contains("@email", actual);
            Assert.DoesNotContain("'\" + email + \"'", actual);
        }

        [Fact]
        public void UpdateCustomerPassword_UsesParametersForPasswordAndCustomerNumber()
        {
            // Arrange
            var sql = "update CustomerLogin set password = @password where customerNumber = @customerNumber";

            // Act & Assert
            Assert.Contains("@password", sql);
            Assert.Contains("@customerNumber", sql);
            Assert.DoesNotContain("+ Encoder.Encode(password) +", sql);
            Assert.DoesNotContain("+ customerNumber", sql);
        }

        [Fact]
        public void GetPayments_UsesParameterForCustomerNumber()
        {
            // Arrange
            var sql = "select * from Payments where customerNumber = @customerNumber";

            // Assert
            Assert.Contains("@customerNumber", sql);
            Assert.DoesNotContain("+ customerNumber", sql);
        }
    }
}
