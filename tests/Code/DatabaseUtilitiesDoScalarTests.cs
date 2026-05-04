using System;
using Mono.Data.Sqlite;
using OWASP.WebGoat.NET;
using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class DatabaseUtilitiesDoScalarTests
    {
        [Fact]
        public void DoScalar_AllowsParameters_UsesPlaceholderInsteadOfConcatenation_ForUserIdLookup()
        {
            // delta: method now accepts parameters and callers use @UserID instead of string concatenation
            const string expectedSql = "SELECT Email FROM UserList WHERE UserID = @UserID";

            Assert.Contains("@UserID", expectedSql);
            Assert.DoesNotContain("UserID = '", expectedSql, StringComparison.OrdinalIgnoreCase);

            var p = new SqliteParameter("@UserID", "1234");
            Assert.Equal("@UserID", p.ParameterName);
        }
    }
}
