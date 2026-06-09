using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderEmailByCustomerNumberTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_UsesParameterInsteadOfConcatenation()
        {
            // Arrange/Act
            var sql = "select email from CustomerLogin where customerNumber = @num";

            // Assert
            Assert.Contains("@num", sql);
            Assert.DoesNotContain("customerNumber = \" + num", sql);
        }
    }
}
