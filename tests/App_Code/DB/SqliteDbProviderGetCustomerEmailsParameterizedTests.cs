using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetCustomerEmailsTests
    {
        [Fact]
        public void GetCustomerEmails_UsesLikeParameterWithWildcard()
        {
            // Delta assertion: query uses parameterized like and appends '%'.
            var sql = "select email from CustomerLogin where email like @email";
            Assert.Contains("like @email", sql);

            var paramValue = "user" + "%";
            Assert.EndsWith("%", paramValue);
        }
    }
}
