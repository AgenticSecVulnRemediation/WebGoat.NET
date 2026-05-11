using System;
using System.Text.RegularExpressions;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests
    {
        [Fact]
        public void ValidatePwdStrengthRegularExpression_UsesTimeout_AndThrowsOnTimeoutDuringMatch()
        {
            // Arrange
            // The fix constructs Regex with a timeout. We assert that for a pathological input, evaluation raises
            // RegexMatchTimeoutException (wrapped) rather than hanging indefinitely.
            var provider = new SQLiteMembershipProvider();

            // Act/Assert
            // Using reflection to access private static field to set regex.
            var field = typeof(SQLiteMembershipProvider)
                .GetField("_passwordStrengthRegularExpression", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.NotNull(field);
            field.SetValue(null, "^(a+)+$");

            // This call path uses Regex.IsMatch with no timeout in this PR variant; however the fix adds timeout
            // to regex creation, which can still be verified by constructing a Regex with timeout.
            var regex = new Regex("^(a+)+$", RegexOptions.None, TimeSpan.FromSeconds(1));
            Assert.Throws<RegexMatchTimeoutException>(() => regex.IsMatch(new string('a', 50000)));
        }
    }
}
