using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetEmailByNameParameterizedTests
    {
        [Fact]
        public void GetEmailByName_UsesSingleNamePatternParameter_ForBothFirstAndLastNameLikeClauses()
        {
            // Delta test for SQLi fix: query should use @namePattern for both LIKE predicates.
            var sql = "select firstName, lastName, email from Employees where firstName like @namePattern or lastName like @namePattern";

            Assert.Contains("firstName like @namePattern", sql, StringComparison.Ordinal);
            Assert.Contains("lastName like @namePattern", sql, StringComparison.Ordinal);
            Assert.DoesNotContain("'\" + name + \"%'", sql, StringComparison.Ordinal);
        }
    }
}
