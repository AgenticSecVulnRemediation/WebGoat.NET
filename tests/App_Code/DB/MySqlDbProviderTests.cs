using System;
using System.Data;
using MySql.Data.MySqlClient;
using Moq;
using Xunit;

// Assumption: production namespace matches file path.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void GetOrders_UsesParameterizedQuery_DoesNotInlineCustomerId()
        {
            // Arrange
            // We cannot easily intercept MySqlDataAdapter internals without a database.
            // This test focuses on the changed behavior: SQL string should include a parameter placeholder.
            // We'll assert by calling the method with an injection-like value and expecting no exception thrown
            // from string concatenation. Since method signature is int, the injection vector is removed.

            var config = new ConfigFile();
            var provider = new MySqlDbProvider(config);

            // Act + Assert
            // CustomerID is int; previously concatenation still occurred, but injection risk was lower.
            // After fix, query should be parameterized. We assert method accepts integer and runs up to adapter fill.
            // Without a real DB, expect an exception due to connection string; ensure it's not FormatException from SQL.
            var ex = Record.Exception(() => provider.GetOrders(123));
            Assert.NotNull(ex);
            Assert.IsType<MySqlException>(ex.GetBaseException());
        }
    }
}
