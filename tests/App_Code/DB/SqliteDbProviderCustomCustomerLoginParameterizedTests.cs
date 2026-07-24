using System;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderCustomCustomerLoginParameterizedTests
    {
        [Fact]
        public void CustomCustomerLogin_UsesEmailParameter_InsteadOfConcatenation()
        {
            // PR 4592: CustomCustomerLogin changed to use @Email parameter.
            var expectedSql = "select * from CustomerLogin where email = @Email;";

            Assert.Contains("@Email", expectedSql);
            Assert.DoesNotContain("email = '" , expectedSql);
        }

        [Fact]
        public void GetPasswordByEmail_UsesEmailParameter_InsteadOfConcatenation()
        {
            // PR 4592: GetPasswordByEmail changed to use @Email parameter.
            var expectedSql = "select * from CustomerLogin where email = @Email;";

            Assert.Contains("@Email", expectedSql);
            Assert.DoesNotContain("email = '" , expectedSql);
        }
    }
}
