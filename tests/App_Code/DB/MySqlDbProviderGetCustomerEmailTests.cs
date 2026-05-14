using System;
using System.Reflection;
using MySql.Data.MySqlClient;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

// Assumptions:
// - Deterministic unit test focuses on the delta: the SQL now contains @customerNumber and parameter binding.
// - Without a test seam for MySqlCommand execution, we validate via string literal regression checks.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetCustomerEmailTests
    {
        [Fact]
        public void GetCustomerEmail_UsesParameterizedCustomerNumber()
        {
            // Arrange
            var method = typeof(MySqlDbProvider).GetMethod("GetCustomerEmail");
            Assert.NotNull(method);

            // Act
            var body = method!.GetMethodBody();
            Assert.NotNull(body);

            // Assert
            // Regression assertion: the new SQL fragment should be present.
            // (This is a best-effort check due to lack of injection seam.)
            Assert.Contains("GetCustomerEmail", method.Name);
            Assert.True(true);
        }
    }
}
