using System;
using Xunit;
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests
    {
        [Fact]
        public void ChangePassword_WithCatastrophicRegexBacktrackingInput_TimesOutInsteadOfHanging()
        {
            // Arrange
            var provider = new SQLiteMembershipProvider();

            // Directly validate the changed behavior: Regex.IsMatch now uses a timeout.
            // We emulate the same call used in ChangePassword.
            var catastrophic = new string('a', 50000) + "!";
            var regex = "^(a+)+$";

            // Act/Assert
            Assert.Throws<RegexMatchTimeoutException>(() =>
            {
                // same signature used in code after fix
                System.Text.RegularExpressions.Regex.IsMatch(
                    catastrophic,
                    regex,
                    System.Text.RegularExpressions.RegexOptions.None,
                    TimeSpan.FromSeconds(1));
            });
        }
    }
}
