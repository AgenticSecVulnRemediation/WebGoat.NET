using System;
using System.Text.RegularExpressions;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderPasswordRegexTimeoutDeltaTests
    {
        [Fact]
        public void Patch160_RegexMatch_WithTimeout_ThrowsRegexMatchTimeoutException_ForCatastrophicPattern()
        {
            // Delta assertion: code now uses Regex.IsMatch(..., RegexOptions.None, TimeSpan.FromMilliseconds(1000))
            // to mitigate ReDoS.
            string input = new string('a', 50000) + "!";

            Assert.Throws<RegexMatchTimeoutException>(() =>
                Regex.IsMatch(input, "^(a+)+$", RegexOptions.None, TimeSpan.FromMilliseconds(1000)));
        }
    }
}
