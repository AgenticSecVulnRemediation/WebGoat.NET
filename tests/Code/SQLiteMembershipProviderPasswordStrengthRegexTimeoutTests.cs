using System;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;

using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderPasswordStrengthRegexTimeoutTests
    {
        [Fact]
        public void CreateUser_UsesRegexTimeoutToMitigateReDoS()
        {
            // Delta regression test: Regex.IsMatch overload now includes timeout.
            var asmText = Encoding.UTF8.GetString(System.IO.File.ReadAllBytes(typeof(SQLiteMembershipProvider).Assembly.Location));

            Assert.Contains("TimeSpan.FromMilliseconds(1000)", asmText);
            Assert.Contains("RegexOptions.None", asmText);
        }
    }
}
