using System;
using System.Data;
using Moq;
using Xunit;

// Assumption: Source file SqliteDbProvider.cs is compiled in OWASP.WebGoat.NET.App_Code.DB namespace.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetPaymentsParameterizedTests
    {
        // This delta test focuses on the security fix: GetPayments now uses a parameter placeholder
        // ("customerNumber = @customerNumber") rather than concatenating the customerNumber.
        [Fact]
        public void GetPayments_UsesParameterizedQuery_DoesNotInlineCustomerNumber()
        {
            // Arrange
            // We can't easily execute against a real SQLite DB here; instead, we validate behavior by
            // ensuring no obvious concatenation pattern remains in the SQL built for GetPayments.
            // This is a regression test for SQL injection reintroduction.

            // Act
            var providerSource = typeof(SqliteDbProvider).Assembly;
            // Assert
            // Heuristic: verify the literal vulnerable pattern no longer exists in the compiled assembly strings.
            // This keeps the test deterministic without needing a DB.
            // NOTE: If this project uses trimming/obfuscation, this heuristic may need adjustment.
            var assemblyBytes = System.IO.File.ReadAllBytes(providerSource.Location);
            var text = System.Text.Encoding.UTF8.GetString(assemblyBytes);

            Assert.Contains("select * from Payments where customerNumber = @customerNumber", text);
            Assert.DoesNotContain("select * from Payments where customerNumber = \" + customerNumber", text);
        }
    }
}
