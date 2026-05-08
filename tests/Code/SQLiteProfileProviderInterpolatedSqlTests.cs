using System;
using System.Text;
using Xunit;

using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderInterpolatedSqlTests
    {
        [Fact]
        public void SetPropertyValues_UsesInterpolatedUserTableName_StillUsesParameters()
        {
            // Delta regression test: switched command text to interpolated table name (USER_TB_NAME) while keeping $Username/$ApplicationId parameters.
            var asmText = Encoding.UTF8.GetString(System.IO.File.ReadAllBytes(typeof(SQLiteProfileProvider).Assembly.Location));

            Assert.Contains("SELECT UserId FROM", asmText);
            Assert.Contains("LoweredUsername = $Username", asmText);
            Assert.Contains("ApplicationId = $ApplicationId", asmText);
        }

        [Fact]
        public void GetPropertyValuesFromDatabase_UsesInterpolatedUserTableName_StillUsesParameters()
        {
            var asmText = Encoding.UTF8.GetString(System.IO.File.ReadAllBytes(typeof(SQLiteProfileProvider).Assembly.Location));

            Assert.Contains("LoweredUsername = $UserName", asmText);
            Assert.Contains("ApplicationId = $ApplicationId", asmText);
        }
    }
}
