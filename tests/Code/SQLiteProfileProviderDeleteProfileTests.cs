using System;
using System.Reflection;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderDeleteProfileTests
    {
        [Fact]
        public void DeleteProfile_StillExists_AfterParameterMarkerFix()
        {
            // The vulnerability fix changed parameter binding from "$UserId" to "@UserId".
            // This test ensures the method signature remains available after the change.
            var method = typeof(TechInfoSystems.Data.SQLite.SQLiteProfileProvider)
                .GetMethod("DeleteProfile", BindingFlags.NonPublic | BindingFlags.Static);

            Assert.NotNull(method);
        }
    }
}
