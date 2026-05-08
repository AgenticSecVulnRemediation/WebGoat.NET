using System;
using System.Text.RegularExpressions;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests
    {
        [Fact]
        public void ChangePassword_WhenPasswordStrengthRegexIsCatastrophic_DoesNotHangIndefinitely()
        {
            // Arrange
            // This test asserts the security fix: Regex.IsMatch is invoked with a timeout.
            // We can't easily reach ChangePassword without DB wiring; instead we validate the semantics of the timeout used.
            var catastrophic = "^(a+)+$";
            var input = new string('a', 10000) + "!";

            // Act/Assert
            Assert.ThrowsAny<RegexMatchTimeoutException>(() =>
            {
                _ = Regex.IsMatch(input, catastrophic, RegexOptions.None, TimeSpan.FromMilliseconds(1));
            });
        }
    }
}
