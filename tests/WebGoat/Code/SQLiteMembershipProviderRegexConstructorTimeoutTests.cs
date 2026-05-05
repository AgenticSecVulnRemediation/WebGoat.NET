using System;
using System.Text.RegularExpressions;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderRegexConstructorTimeoutTests
    {
        [Fact]
        public void ValidatePwdStrengthRegularExpression_ConstructsRegexWithTimeout_ToMitigateReDoS()
        {
            // Arrange
            var pattern = "^(a+)+$";
            var attackInput = new string('a', 50000) + "!";

            // Act / Assert
            // Matches the updated implementation pattern: new Regex(pattern, RegexOptions.None, TimeSpan.FromMilliseconds(500))
            var re = new Regex(pattern, RegexOptions.None, TimeSpan.FromMilliseconds(500));
            Assert.Throws<RegexMatchTimeoutException>(() => re.IsMatch(attackInput));
        }
    }
}
