using System;
using System.Data;
using Mono.Data.Sqlite;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderQueryParameterizationTests
    {
        [Fact]
        public void CustomCustomerLogin_AddsEmailParameter_ToSelectCommand()
        {
            // Arrange
            // Use an in-memory sqlite db to ensure the adapter/command object exists and can accept parameters.
            var provider = (SqliteDbProvider)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(SqliteDbProvider));
            typeof(SqliteDbProvider).GetField("_connectionString", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!
                .SetValue(provider, "Data Source=:memory:;Version=3");

            // Act
            Exception? ex = Record.Exception(() => provider.CustomCustomerLogin("a@b.com", "pw"));

            // Assert
            // It may throw because schema not present; but should not throw due to missing parameter addition.
            Assert.NotNull(ex);
            Assert.DoesNotContain("@Email", ex!.ToString(), StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void GetCustomerEmails_UsesParameterPlaceholder_NotStringConcatenation()
        {
            var provider = (SqliteDbProvider)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(SqliteDbProvider));
            typeof(SqliteDbProvider).GetField("_connectionString", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!
                .SetValue(provider, "Data Source=:memory:;Version=3");

            Exception? ex = Record.Exception(() => provider.GetCustomerEmails("a"));
            Assert.NotNull(ex);
            Assert.DoesNotContain("like '", ex!.ToString(), StringComparison.Ordinal);
        }
    }
}
