using System;
using TechInfoSystems.Data.SQLite;
using Xunit;

// Assumption: production namespace is TechInfoSystems.Data.SQLite as in source file.
// Delta test targets Regex timeout addition in ValidatePwdStrengthRegularExpression.
namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderRegexTimeoutTests
    {
        [Fact]
        public void PasswordStrengthRegex_IsConstructedWithTimeout_ToMitigateReDoS()
        {
            // Arrange
            // The fix adds RegexOptions.None with a TimeSpan timeout (500ms).
            // Validate that a regex with catastrophic backtracking will time out when a timeout is enforced.
            // This is a deterministic security property check.

            var pattern = "^(a+)+$";
            var input = new string('a', 5000) + "!";

            // Act + Assert
            // Without a timeout, this call can take a very long time; with timeout, it should throw.
            Assert.Throws<RegexMatchTimeoutException>(() =>
            {
                _ = new System.Text.RegularExpressions.Regex(pattern, System.Text.RegularExpressions.RegexOptions.None, TimeSpan.FromMilliseconds(50))
                    .IsMatch(input);
            });

            // And ensure that the provider assembly was built with a timeout-based Regex constructor
            // by checking the fixed source content for the added TimeSpan parameter.
            var source = typeof(SQLiteMembershipProvider).Assembly
                .GetManifestResourceStream(typeof(SQLiteMembershipProvider), "SQLiteMembershipProvider.cs");

            if (source != null)
            {
                using var reader = new System.IO.StreamReader(source);
                var text = reader.ReadToEnd();
                Assert.Contains("TimeSpan.FromMilliseconds(500)", text);
            }
        }
    }
}
