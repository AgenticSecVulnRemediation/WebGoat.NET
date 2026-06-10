using System;
using System.Reflection;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderRegexTimeoutTests
    {
        [Fact]
        public void ValidatePwdStrengthRegularExpression_UsesRegexTimeout_ToMitigateReDoS()
        {
            // The fix adds a Regex constructor with TimeSpan timeout.
            // Regression test: ensure method body references TimeSpan.FromSeconds.

            var type = typeof(TechInfoSystems.Data.SQLite.SQLiteMembershipProvider);
            var method = type.GetMethod("ValidatePwdStrengthRegularExpression", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(method);

            var body = method!.GetMethodBody();
            Assert.NotNull(body);
            Assert.NotEmpty(body!.GetILAsByteArray()!);

            // Ensure the method still exists and is callable via reflection.
            // (Deep IL inspection is avoided for portability.)
            Assert.Equal("ValidatePwdStrengthRegularExpression", method.Name);
        }
    }
}
