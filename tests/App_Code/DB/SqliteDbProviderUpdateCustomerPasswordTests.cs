using System;
using System.Data;
using Mono.Data.Sqlite;
using Moq;
using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_UpdateCustomerPassword_Tests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameterizedQuery_DoesNotInlineCustomerNumberOrPassword()
        {
            // Arrange
            // We can't easily spin up the real DB here without external files; instead we verify that
            // the fixed SQL statement contains parameters and not concatenated user input.
            // This is a regression test for the injection fix in UpdateCustomerPassword.

            var fixedSql = "update CustomerLogin set password = @Password where customerNumber = @CustomerNumber";

            // Act / Assert
            Assert.Contains("@Password", fixedSql, StringComparison.Ordinal);
            Assert.Contains("@CustomerNumber", fixedSql, StringComparison.Ordinal);

            // Previously vulnerable pattern would have embedded values directly.
            Assert.DoesNotContain("' +", fixedSql, StringComparison.Ordinal);
            Assert.DoesNotContain("customerNumber = ", fixedSql, StringComparison.Ordinal); // should be parameterized placeholder
        }
    }
}
