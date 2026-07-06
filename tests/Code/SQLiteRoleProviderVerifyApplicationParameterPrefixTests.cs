using System;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderVerifyApplicationParameterPrefixTests
    {
        [Fact]
        public void VerifyApplication_UsesAtPrefixedParameters_ForInsert()
        {
            // Delta: $ApplicationId/$ApplicationName/$Description changed to @ApplicationId/@ApplicationName/@Description
            var src = System.IO.File.ReadAllText("WebGoat/Code/SQLiteRoleProvider.cs");

            Assert.Contains("VALUES (@ApplicationId, @ApplicationName, @Description)", src);
            Assert.Contains("AddWithValue (\"@ApplicationId\"", src);
            Assert.Contains("AddWithValue (\"@ApplicationName\"", src);
            Assert.Contains("AddWithValue (\"@Description\"", src);

            Assert.DoesNotContain("VALUES ($ApplicationId, $ApplicationName, $Description)", src);
        }
    }
}
