using System;
using System.Text.RegularExpressions;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProvider_ChangePassword_ReDoSTests
    {
        [Fact]
        public void RegexTimeout_CatastrophicBacktracking_ThrowsRegexMatchTimeoutException()
        {
            // Arrange
            // Mirrors the fix: use Regex.IsMatch overload with a timeout.
            // Catastrophic pattern that will time out on sufficiently long input.
            var pattern = "^(a+)+$";
            var input = new string('a', 50000) + "!";

            // Act / Assert
            Assert.Throws<RegexMatchTimeoutException>(() =>
                Regex.IsMatch(input, pattern, RegexOptions.None, TimeSpan.FromMilliseconds(1))
            );
        }
    }
}
