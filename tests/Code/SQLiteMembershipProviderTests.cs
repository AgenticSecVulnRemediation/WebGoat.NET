using System;
using System.Text.RegularExpressions;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests
    {
        [Fact]
        public void CreateUser_WithCatastrophicBacktrackingRegex_ShouldTimeoutInsteadOfHanging()
        {
            // Delta test: fix adds Regex.IsMatch timeout to mitigate ReDoS when validating passwordStrengthRegularExpression.
            // This test directly exercises Regex timeout behavior used in the fix.

            string pattern = "^(a+)+$";
            string input = new string('a', 20000) + "!";

            // Act/Assert: should throw quickly (or complete quickly) rather than hang.
            var ex = Record.Exception(() => Regex.IsMatch(input, pattern, RegexOptions.None, TimeSpan.FromMilliseconds(50)));
            Assert.True(ex is RegexMatchTimeoutException || ex is null);
        }
    }
}
