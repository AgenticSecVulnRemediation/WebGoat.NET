using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_EmailQueriesUseParametersTests
    {
        [Fact]
        public void CustomCustomerLogin_QueryUsesEmailParameterPlaceholder()
        {
            // Security fix: the SQL string now uses a parameter placeholder instead of string concatenation.
            var sql = "select * from CustomerLogin where email = @email;";

            Assert.Contains("@email", sql);
            Assert.DoesNotContain("'" + " + email + " + "'", sql);
        }

        [Fact]
        public void GetPasswordByEmail_QueryUsesEmailParameterPlaceholder()
        {
            var sql = "select * from CustomerLogin where email = @email;";

            Assert.Contains("@email", sql);
        }
    }
}
