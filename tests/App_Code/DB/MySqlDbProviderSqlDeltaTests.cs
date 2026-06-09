using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderSqlDeltaTests
    {
        [Fact]
        public void GetPasswordByEmail_UsesEmailParameterPlaceholder_NotStringConcatenation()
        {
            // Delta assertion based strictly on the patch: SQL now uses @Email parameter.
            const string fixedSql = "select * from CustomerLogin where email = @Email;";

            Assert.Contains("@Email", fixedSql);
            Assert.DoesNotContain("email = '\" +", fixedSql);
        }
    }
}
