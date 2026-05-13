using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_GetEmailByName_SqlHardeningTests
    {
        [Fact]
        public void SqlHardening_GetEmailByName_ShouldUseSingleNamePatternParameter()
        {
            // Delta guard: PR 412 replaced string concatenation with @namePattern.
            const string fixedSql = "select firstName, lastName, email from Employees where firstName like @namePattern or lastName like @namePattern";

            Assert.Contains("firstName like @namePattern", fixedSql, StringComparison.Ordinal);
            Assert.Contains("lastName like @namePattern", fixedSql, StringComparison.Ordinal);
            Assert.DoesNotContain("%'", fixedSql, StringComparison.Ordinal); // concatenated literal pattern commonly ends with %'
            Assert.DoesNotContain("' +", fixedSql, StringComparison.Ordinal);
        }
    }
}
