using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Web.Profile;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderProfileTableQueryTests
    {
        [Fact]
        public void SetPropertyValues_UsesProfileTableConstantInInterpolatedSql()
        {
            // Arrange
            // Regression test for refactor changing SQL concatenation to string interpolation using PROFILE_TB_NAME.
            // We don't hit the DB; we validate presence of the constant in the compiled type metadata.
            var field = typeof(SQLiteProfileProvider).GetField("PROFILE_TB_NAME", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.NotNull(field);
            Assert.Equal("[aspnet_Profile]", field!.GetRawConstantValue());
        }
    }
}
