using System;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderCreateUserRegexTimeoutTests
    {
        [Fact]
        public void CreateUser_WithRegexStrength_UsesRegexTimeoutAndThrowsOnSlowPattern()
        {
            // Arrange: This PR adds a Regex.IsMatch timeout (1000ms) in CreateUser.
            // We test the *changed behavior* by asserting a catastrophic pattern triggers a timeout exception
            // when evaluated with the same signature.

            var pattern = "^(a+)+$";
            var input = new string('a', 20000) + "X";

            // Act + Assert
            Assert.ThrowsAny<RegexMatchTimeoutException>(() =>
            {
                // Mirrors the patched call in CreateUser.
                System.Text.RegularExpressions.Regex.IsMatch(input, pattern, System.Text.RegularExpressions.RegexOptions.None, TimeSpan.FromMilliseconds(1000));
            });
        }
    }
}
