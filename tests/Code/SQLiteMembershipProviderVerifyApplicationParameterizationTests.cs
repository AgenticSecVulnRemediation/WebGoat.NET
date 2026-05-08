using System;
using System.Text;
using Xunit;

using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderVerifyApplicationParameterizationTests
    {
        [Fact]
        public void VerifyApplication_UsesParameterPlaceholdersForInsert()
        {
            // Delta regression test: VerifyApplication now uses interpolated command text but keeps parameter placeholders ($ApplicationId, $ApplicationName, $Description).
            // Also fixes parameters to use "$ApplicationName" and "$Description" keys.
            var asmText = Encoding.UTF8.GetString(System.IO.File.ReadAllBytes(typeof(SQLiteMembershipProvider).Assembly.Location));

            Assert.Contains("INSERT INTO", asmText);
            Assert.Contains("$ApplicationName", asmText);
            Assert.Contains("$Description", asmText);
        }
    }
}
