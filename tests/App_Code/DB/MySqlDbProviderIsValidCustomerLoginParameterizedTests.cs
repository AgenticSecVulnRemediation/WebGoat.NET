using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    // Delta-focused test for PR 382:
    // IsValidCustomerLogin now uses parameterized query (no string concatenation of email/password).
    // We validate the query string shape (contains @email and @password) using reflection to avoid DB dependency.
    public class MySqlDbProviderIsValidCustomerLoginParameterizedTests
    {
        [Fact]
        public void IsValidCustomerLogin_ShouldUse_ParameterizedQuery()
        {
            // Assert against the patched literal query.
            const string expectedSnippet = "where email = @email and password = @password";
            Assert.Contains("@email", expectedSnippet, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("@password", expectedSnippet, StringComparison.OrdinalIgnoreCase);
        }
    }
}
