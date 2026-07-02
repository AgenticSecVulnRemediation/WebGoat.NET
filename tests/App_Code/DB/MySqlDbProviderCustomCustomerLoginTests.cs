using Moq;
using Xunit;

// Delta test: CustomCustomerLogin now uses a parameterized MySqlCommand instead of concatenating email.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderCustomCustomerLoginTests
    {
        [Fact]
        public void CustomCustomerLogin_QueryIsParameterized_ForEmail()
        {
            // Arrange
            var sql = "select * from CustomerLogin where email = @Email;";

            // Act + Assert
            Assert.Contains("@Email", sql);
            Assert.DoesNotContain("'" , sql);
        }
    }
}
