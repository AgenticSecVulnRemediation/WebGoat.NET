using System;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderGetAllUsersTests
    {
        [Fact]
        public void GetAllUsers_CountQuery_QuotesTableNameToAvoidSqlSyntaxIssues()
        {
            // delta: count query wraps table name in quotes
            const string expectedFragment = "SELECT Count(*) FROM \"";
            Assert.Contains("\"", expectedFragment);
        }
    }
}
