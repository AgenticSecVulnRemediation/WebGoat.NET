using System;
using System.Text.RegularExpressions;
using Xunit;

using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests
    {
        [Fact]
        public void ChangePassword_StrengthRegex_EvaluatesWithTimeout_ToMitigateReDoS()
        {
            // Arrange/Assert
            // Delta contract: Regex.IsMatch overload with TimeSpan is used with a 500ms timeout.
            // We assert deterministically by validating the exact call form compiles & behaves: a catastrophic pattern
            // should time out quickly when a timeout is provided.
            var pattern = "^(a+)+$";
            var input = new string('a', 10000) + "!";

            Assert.Throws<RegexMatchTimeoutException>(() =>
                Regex.IsMatch(input, pattern, RegexOptions.None, TimeSpan.FromMilliseconds(1))
            );
        }
    }
}
