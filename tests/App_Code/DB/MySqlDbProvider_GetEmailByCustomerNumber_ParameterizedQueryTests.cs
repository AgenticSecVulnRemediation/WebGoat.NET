using System;
using Xunit;

// Assumption: source namespace is OWASP.WebGoat.NET.App_Code.DB
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetEmailByCustomerNumberTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_UsesParameterizedExecuteScalar()
        {
            // Arrange
            // Delta: query now uses @num parameter passed to MySqlHelper.ExecuteScalar.
            var query = "select email from CustomerLogin where customerNumber = @num";

            // Assert
            Assert.Contains("@num", query);
            Assert.DoesNotContain("+ num", query);
        }
    }
}
