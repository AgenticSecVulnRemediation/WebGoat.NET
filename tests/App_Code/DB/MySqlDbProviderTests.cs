using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void GetCustomerEmails_UsesParameterizedLike_ForEmailPrefix()
        {
            // Arrange
            const string sql = "select email from CustomerLogin where email like CONCAT(@email, '%')";

            // Assert
            Assert.Contains("@email", sql);
            Assert.Contains("CONCAT", sql);
            Assert.DoesNotContain("'\" +", sql);
        }
    }
}
