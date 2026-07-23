using System;
using System.Reflection;
using Xunit;

// NOTE: Namespace inferred from source file path `WebGoat/Code/SQLiteMembershipProvider.cs`
// Source namespace in file is `TechInfoSystems.Data.SQLite`.
namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests_RegexTimeout
    {
        [Fact]
        public void ValidatePwdStrengthRegularExpression_WhenRegexIsPathological_DoesNotHang()
        {
            // Arrange
            // Diff change: ValidatePwdStrengthRegularExpression now compiles regex with a timeout.
            // We set a catastrophic-backtracking pattern and ensure the validation completes quickly.
            var providerType = typeof(SQLiteMembershipProvider);

            var regexField = providerType.GetField("_passwordStrengthRegularExpression", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(regexField);

            // A known ReDoS pattern
            regexField!.SetValue(null, "^(a+)+$");

            var method = providerType.GetMethod("ValidatePwdStrengthRegularExpression", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(method);

            // Act/Assert
            // If there is no timeout when compiling, some runtimes/configs can spend excessive time.
            // We assert it completes within a reasonable bound.
            var start = DateTime.UtcNow;
            method!.Invoke(null, null);
            var elapsed = DateTime.UtcNow - start;

            Assert.True(elapsed < TimeSpan.FromSeconds(2), $"Regex validation took too long: {elapsed}");
        }
    }
}
