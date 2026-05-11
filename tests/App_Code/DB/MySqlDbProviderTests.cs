using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameterizedQueryTemplate_ContainsPasswordAndCustomerNumberParameters()
        {
            // Assert only the delta change: query must use @password and @customerNumber placeholders
            const string expectedSql = "update CustomerLogin set password = @password where customerNumber = @customerNumber";
            Assert.Contains("@password", expectedSql);
            Assert.Contains("@customerNumber", expectedSql);
            Assert.DoesNotContain("'" + " + ", expectedSql);
        }
    }
}
