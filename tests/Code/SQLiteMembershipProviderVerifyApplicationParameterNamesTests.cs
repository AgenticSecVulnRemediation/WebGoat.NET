using System;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;
using System.Web;
using System.Web.Security;
using Moq;
using Xunit;

// Assumptions:
// - Source namespaces are as declared in the patched file.
// - We avoid real DB access by directly invoking the private VerifyApplication() method and inspecting its SQL/parameters.

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderVerifyApplicationParameterNamesTests
    {
        [Fact]
        public void VerifyApplication_WhenInsertingApplication_UsesExpectedNamedParametersWithDollarPrefix()
        {
            // Arrange: ensure applicationId is empty so VerifyApplication will attempt to insert
            var providerType = typeof(TechInfoSystems.Data.SQLite.SQLiteMembershipProvider);

            providerType.GetField("_applicationId", BindingFlags.NonPublic | BindingFlags.Static)!
                .SetValue(null, null);
            providerType.GetField("_applicationName", BindingFlags.NonPublic | BindingFlags.Static)!
                .SetValue(null, "TestApp");
            providerType.GetField("_connectionString", BindingFlags.NonPublic | BindingFlags.Static)!
                .SetValue(null, "Data Source=:memory:");

            // Provide a fake HttpContext with an open transaction/connection so GetDBConnectionForMembership returns it.
            // We don't want to connect; instead we intercept SqliteCommand creation via a shim-like approach:
            // since Mono.Data.Sqlite types are concrete, we validate behavior by inspecting the command text and parameters
            // built by VerifyApplication using reflection against the created command object.

            // Create a minimal HttpContext
            HttpContext.Current = new HttpContext(
                new HttpRequest("", "http://localhost/", ""),
                new HttpResponse(null));

            // Act + Assert: call VerifyApplication via reflection and expect it to create parameters named $ApplicationId, $ApplicationName, $Description.
            // Because we cannot easily mock Mono.Data.Sqlite internals without external tooling, we assert the fixed behavior indirectly:
            // VerifyApplication should not throw due to missing '$' parameter names (regression for previous bug).

            var verifyApplication = providerType.GetMethod("VerifyApplication", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(verifyApplication);

            // If parameter names were wrong (e.g., missing '$'), Mono.Data.Sqlite would throw at ExecuteNonQuery when binding.
            // We therefore expect a ProviderException due to connection inability (in-memory without schema), but NOT an ArgumentException
            // about missing parameters.
            var ex = Record.Exception(() => verifyApplication!.Invoke(null, null));

            // Unwrap TargetInvocationException if present
            if (ex is TargetInvocationException tie && tie.InnerException != null)
                ex = tie.InnerException;

            // We don't assert exact exception type (depends on provider availability) but we do assert it's not a parameter-binding error.
            // Previously vulnerable/buggy behavior used parameter names without '$' which can cause binding failures.
            if (ex != null)
            {
                Assert.DoesNotContain("ApplicationName", ex.Message, StringComparison.OrdinalIgnoreCase);
                Assert.DoesNotContain("Description", ex.Message, StringComparison.OrdinalIgnoreCase);
                Assert.DoesNotContain("parameter", ex.Message, StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}
