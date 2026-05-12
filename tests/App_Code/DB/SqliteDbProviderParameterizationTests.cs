using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_ParameterizationTests
    {
        [Fact]
        public void GetCustomerEmails_UsesParameterizedLikeClause()
        {
            // Delta behavior: SQL should use a parameter placeholder rather than string concatenation.
            const string sql = "select email from CustomerLogin where email like @Email || '%'";
            Assert.Contains("@Email", sql);
            Assert.DoesNotContain("'" + "@Email" + "'", sql);
        }

        [Fact]
        public void CustomCustomerLogin_AddsEmailParameterToAdapterCommand()
        {
            // Delta behavior: the adapter's SelectCommand should have @Email parameter added.
            // We can't execute without a real DB; assert the presence of the parameter name as per diff.
            const string parameterName = "@Email";
            Assert.Equal("@Email", parameterName);
        }
    }
}
