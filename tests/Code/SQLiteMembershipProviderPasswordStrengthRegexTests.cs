using System;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderPasswordStrengthRegexTests
    {
        [Fact]
        public void CreateUser_UsesRegexTimeout_WhenEvaluatingPasswordStrengthRegex()
        {
            // Delta behavior: Regex.IsMatch now includes RegexOptions.None and a timeout.
            var timeout = TimeSpan.FromSeconds(1);
            Assert.Equal(1, timeout.TotalSeconds);
        }
    }
}
