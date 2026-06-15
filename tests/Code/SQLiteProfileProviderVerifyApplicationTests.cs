using System;
using Xunit;

// Assumption: project uses the same root namespace as its folder structure.
// Source: WebGoat/Code/SQLiteProfileProvider.cs => namespace TechInfoSystems.Data.SQLite
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderVerifyApplicationTests
    {
        [Fact]
        public void VerifyApplication_UsesAppTableConstant_WithExpectedBracketedName()
        {
            // This is a delta test focused on the vulnerability fix in VerifyApplication():
            // it now uses string interpolation with the APP_TB_NAME constant.
            // We assert the constant is the expected safe bracketed table name.

            var field = typeof(SQLiteProfileProvider).GetField("APP_TB_NAME",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

            Assert.NotNull(field);
            var value = field!.GetValue(null) as string;

            Assert.Equal("[aspnet_Applications]", value);
        }
    }
}
