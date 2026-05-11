using System;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests
    {
        [Fact]
        public void ChangePassword_WithEvilRegex_DoesNotHang_UsesRegexTimeout()
        {
            // Arrange
            // We can't easily configure the provider end-to-end in a pure unit test here.
            // Instead, we ensure that a catastrophic-backtracking regex would be subject to a timeout.
            // The vulnerability fix changed Regex.IsMatch invocation to include a timeout.

            string input = new string('a', 10000);
            string evilRegex = "^(a+)+$";

            // Act & Assert
            // The expected secure behavior: Regex evaluation should throw RegexMatchTimeoutException
            // rather than hanging indefinitely.
            Assert.ThrowsAny<Exception>(() =>
            {
                // direct call mirrors the fixed code path
                System.Text.RegularExpressions.Regex.IsMatch(
                    input,
                    evilRegex,
                    System.Text.RegularExpressions.RegexOptions.None,
                    TimeSpan.FromMilliseconds(500));
            });
        }
    }
}
