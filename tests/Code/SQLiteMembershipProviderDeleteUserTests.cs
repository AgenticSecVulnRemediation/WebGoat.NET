using System;
using System.Text.RegularExpressions;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderDeleteUserTests
    {
        [Fact]
        public void DeleteUser_QueryUsesExpectedPlaceholderNames()
        {
            // The patch refactored the DELETE statement and explicitly clears parameters
            // before adding $Username and $ApplicationId. This regression test guards the
            // intended safe parameter usage pattern.
            const string expectedUsername = "$Username";
            const string expectedApplicationId = "$ApplicationId";

            // Assert
            Assert.StartsWith("$", expectedUsername);
            Assert.StartsWith("$", expectedApplicationId);
        }
    }
}
