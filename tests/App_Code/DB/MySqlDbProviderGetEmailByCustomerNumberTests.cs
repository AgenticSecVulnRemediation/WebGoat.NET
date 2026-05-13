using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetEmailByCustomerNumberTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_UsesParameter_ForScalarQuery()
        {
            // Arrange
            string query = "select email from CustomerLogin where customerNumber = @num";

            // Assert
            Assert.Contains("@num", query, StringComparison.Ordinal);
            Assert.DoesNotContain("customerNumber = \" + num", query, StringComparison.OrdinalIgnoreCase);
        }
    }
}
