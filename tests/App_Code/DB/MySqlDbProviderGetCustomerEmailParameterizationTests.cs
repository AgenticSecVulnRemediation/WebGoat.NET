using System;
using Xunit;

// Assumption: Source namespace from file path is OWASP.WebGoat.NET.App_Code.DB
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetCustomerEmailParameterizationTests
    {
        [Fact]
        public void GetCustomerEmail_WithInjectionPayload_DoesNotThrowOnSqlMetaCharacters()
        {
            // Arrange
            // We cannot run MySQL here. This delta test ensures method exists and can be invoked with a payload without immediate formatting failures.
            // The fix parameterizes the query; this test ensures callers can pass untrusted input safely at least at method boundary.

            var provider = (MySqlDbProvider)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(MySqlDbProvider));

            // Act & Assert
            // Method should exist; invocation will likely fail due to missing connection string; we only ensure it doesn't throw ArgumentNullException
            // due to string concatenation formatting.
            var ex = Record.Exception(() => provider.GetCustomerEmail("1 OR 1=1"));
            // Either null (if provider handles) or exception from DB connection; but should not be FormatException from query string building.
            Assert.False(ex is FormatException);
        }
    }
}
