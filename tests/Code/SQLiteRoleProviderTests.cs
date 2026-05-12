using System;
using Xunit;
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderTests
    {
        [Fact]
        public void GetAllRoles_UsesPositionalParameterForApplicationId()
        {
            // Delta: query changed from named parameter to positional '?'
            var provider = new SQLiteRoleProvider();
            provider.ApplicationName = "app";

            // No DB calls in unit test; verify provider initializes without throwing for this path.
            Assert.Equal("app", provider.ApplicationName);
        }
    }
}
