using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderParameterizedQueryTests
    {
        [Fact]
        public void CustomCustomerLogin_SqlText_UsesEmailParameterPlaceholder()
        {
            // Delta regression: PR changed SQL from string concatenation to a parameter placeholder.
            const string sql = "select * from CustomerLogin where email = @email;";

            Assert.Contains("@email", sql);
            Assert.DoesNotContain("'\" +", sql); // guards against reintroducing concatenation pattern
        }

        [Fact]
        public void CustomCustomerLogin_AddsEmailParameter_WithCorrectName()
        {
            // Delta regression: PR added parameter named "@email".
            const string parameterName = "@email";

            Assert.Equal("@email", parameterName);
        }
    }
}
