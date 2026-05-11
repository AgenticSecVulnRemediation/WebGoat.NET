using System;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderTests
    {
        [Fact]
        public void VerifyApplication_UsesFixedAspNetApplicationsTableName()
        {
            // Delta assertion: the diff replaces concatenation with APP_TB_NAME
            // with a literal "[aspnet_Applications]" to ensure correct table name.
            // We verify the constant includes the expected bracketed table name.

            // Act
            var roleTableName = typeof(SQLiteRoleProvider)
                .GetField("APP_TB_NAME", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                ?.GetValue(null) as string;

            // Assert
            Assert.NotNull(roleTableName);
            Assert.Equal("[aspnet_Applications]", roleTableName);
        }
    }
}
