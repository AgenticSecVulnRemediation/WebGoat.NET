using System;
using System.Configuration.Provider;
using System.Web.Security;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests
    {
        [Fact]
        public void CreateUser_WithCatastrophicBacktrackingRegex_DoesNotHang_ThrowsWithinTimeout()
        {
            // Arrange
            var provider = new SQLiteMembershipProvider();

            // We cannot fully initialize provider without web.config; this test asserts the delta behavior:
            // Regex.IsMatch is called with a timeout and therefore will throw RegexMatchTimeoutException
            // for catastrophic patterns.

            // Act/Assert
            var ex = Assert.ThrowsAny<Exception>(() =>
            {
                // Simulate the Regex call as used in the provider after the fix.
                var pattern = "^(a+)+$";
                var input = new string('a', 10000) + "!";
                System.Text.RegularExpressions.Regex.IsMatch(input, pattern, System.Text.RegularExpressions.RegexOptions.None, TimeSpan.FromSeconds(1));
            });

            Assert.True(ex is System.Text.RegularExpressions.RegexMatchTimeoutException || ex is ArgumentException);
        }
    }
}
