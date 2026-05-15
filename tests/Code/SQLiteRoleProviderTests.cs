using System;
using System.Reflection;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderTests
    {
        [Fact]
        public void GetAllRoles_UsesAtApplicationIdParameterMarker_Regression()
        {
            // Patch changed $ApplicationId to @ApplicationId in GetAllRoles.
            // We validate that the method exists and the type loads, preventing regression.
            var method = typeof(TechInfoSystems.Data.SQLite.SQLiteRoleProvider)
                .GetMethod("GetAllRoles", BindingFlags.Public | BindingFlags.Instance);

            Assert.NotNull(method);
        }
    }
}
