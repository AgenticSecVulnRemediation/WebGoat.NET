using System;
using System.Text;
using Xunit;

using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderSetPropertyValuesParameterizationTests
    {
        [Fact]
        public void SetPropertyValues_UsesAtParametersForUserLookup()
        {
            // Delta regression test: changed $Username/$ApplicationId to @Username/@ApplicationId.
            var asmBytes = System.IO.File.ReadAllBytes(typeof(SQLiteProfileProvider).Assembly.Location);
            var asmText = Encoding.UTF8.GetString(asmBytes);

            Assert.Contains("@Username", asmText);
            Assert.Contains("@ApplicationId", asmText);
        }
    }
}
