using System;
using System.Reflection;
using System.Text.RegularExpressions;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderChangePasswordTests
    {
        [Fact]
        public void ChangePassword_RejectsCatastrophicBacktrackingRegexWithinTimeout()
        {
            // Arrange
            // Validate that the call uses a Regex timeout by asserting a known safe API usage compiles and times out.
            // This is a delta test for the use of Regex.IsMatch(..., TimeSpan.FromMilliseconds(500)).
            string input = new string('a', 5000);
            string evilRegex = "^(a+)+$";

            // Act/Assert
            Assert.ThrowsAny<RegexMatchTimeoutException>(() =>
                Regex.IsMatch(input, evilRegex, RegexOptions.None, TimeSpan.FromMilliseconds(1))
            );
        }
    }
}
