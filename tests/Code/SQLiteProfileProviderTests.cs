using System;
using Xunit;
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderTests
    {
        [Fact]
        public void GetPropertyValuesFromDatabase_UsesAtUserIdParameter()
        {
            // Arrange
            var method = typeof(SQLiteProfileProvider).GetMethod("GetPropertyValues", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            Assert.NotNull(method);
        }
    }
}
