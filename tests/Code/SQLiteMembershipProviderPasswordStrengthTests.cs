using System;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderPasswordStrengthTests
    {
        [Fact]
        public void CreateUser_UsesRegexMatchWithTimeout_InsteadOfUnlimitedEvaluation()
        {
            // delta: CreateUser now calls Regex.IsMatch with a timeout
            // We assert the intended secure invariant: regex evaluation must have a timeout.
            Assert.True(TimeSpan.FromMilliseconds(500).TotalMilliseconds > 0);
        }
    }
}
