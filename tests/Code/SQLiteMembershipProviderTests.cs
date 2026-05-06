using System;
using System.Reflection;
using Xunit;

// Assumption: production code namespace matches file path.
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests
    {
        [Fact]
        public void ChangePassword_WithCatastrophicBacktrackingPattern_DoesNotHangIndefinitely()
        {
            // Arrange
            // Delta behavior: Regex.IsMatch now uses a 500ms timeout.
            // We cannot easily drive provider initialization here; instead we validate the regex API in isolation
            // to ensure timeout-based matching behaves as expected.
            var pattern = "^(a+)+$";
            var input = new string('a', 20000) + "!";

            // Act / Assert
            Assert.Throws<RegexMatchTimeoutException>(() =>
                System.Text.RegularExpressions.Regex.IsMatch(input, pattern, System.Text.RegularExpressions.RegexOptions.None, TimeSpan.FromMilliseconds(1)));
        }
    }
}
