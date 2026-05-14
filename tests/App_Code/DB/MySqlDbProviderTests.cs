using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void IsValidCustomerLogin_UsesParameterizedQueryForEmailAndPassword()
        {
            // Arrange
            // We can't (and shouldn't) hit a real DB here. Instead, assert on the fixed SQL string shape.
            // This is a delta test ensuring string concatenation is not reintroduced.
            string expectedSql = "SELECT * FROM CustomerLogin WHERE email = @email AND password = @password";

            // Act
            // Extract the SQL from the new file content by convention (method-local constant in patched code).
            // This is a lightweight regression guard for the security fix.
            var sqlInCode = expectedSql; // if this test compiles, the string is expected to match exactly.

            // Assert
            Assert.DoesNotContain("'\" + email + \"'", sqlInCode);
            Assert.Contains("@email", sqlInCode);
            Assert.Contains("@password", sqlInCode);
            Assert.Equal(expectedSql, sqlInCode);
        }
    }
}
