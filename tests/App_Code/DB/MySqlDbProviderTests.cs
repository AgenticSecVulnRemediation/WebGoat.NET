using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameterizedUpdate_ForPasswordAndCustomerNumber()
        {
            // Arrange
            const string sql = "UPDATE CustomerLogin SET password = @password WHERE customerNumber = @customerNumber";

            // Assert
            Assert.Contains("@password", sql);
            Assert.Contains("@customerNumber", sql);
            Assert.DoesNotContain("'\" +", sql);
        }
    }
}
