using System;
using System.Text;
using Xunit;

using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderGetPropertyValuesParameterizationTests
    {
        [Fact]
        public void GetPropertyValuesFromDatabase_UsesAtUserIdParameter()
        {
            // Delta regression test: query changed from string concatenation with $UserId to formatted SQL with @UserId.
            var asmBytes = System.IO.File.ReadAllBytes(typeof(SQLiteProfileProvider).Assembly.Location);
            var asmText = Encoding.UTF8.GetString(asmBytes);

            Assert.Contains("WHERE UserId = @UserId", asmText);
        }
    }
}
