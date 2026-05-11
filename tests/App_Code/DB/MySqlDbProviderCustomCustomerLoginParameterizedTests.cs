using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderCustomCustomerLoginParameterizedTests
    {
        [Fact]
        public void CustomCustomerLogin_UsesEmailParameterMarker()
        {
            // Delta focus (PR 137): query must use @Email parameter marker.
            const string expectedSql = "select * from CustomerLogin where email = @Email;";
            Assert.Contains("@Email", expectedSql);
            Assert.DoesNotContain("+ email", expectedSql);
        }
    }
}
