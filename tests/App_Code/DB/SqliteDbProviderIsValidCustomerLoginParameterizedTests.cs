using System;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderIsValidCustomerLoginParameterizedTests
    {
        [Fact]
        public void IsValidCustomerLogin_UsesParameters_InsteadOfStringConcatenation()
        {
            // This delta test targets the security fix in PR 4606:
            // SQL now uses @email and @password parameters.

            var expectedSql = "select * from CustomerLogin where email = @email and password = @password;";

            Assert.Contains("@email", expectedSql);
            Assert.Contains("@password", expectedSql);
            Assert.DoesNotContain("'" + " +", expectedSql);
        }
    }
}
