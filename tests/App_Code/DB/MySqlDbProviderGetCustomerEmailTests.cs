using System;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetCustomerEmailTests
    {
        [Fact]
        public void GetCustomerEmail_UsesParameterForCustomerNumber_DoesNotConcatenate()
        {
            // Arrange
            var provider = new MySqlDbProvider(new ConfigFile());
            var customerNumber = "1 OR 1=1";

            // Act
            var ex = Assert.ThrowsAny<Exception>(() => provider.GetCustomerEmail(customerNumber));

            // Assert
            Assert.DoesNotContain("You have an error in your SQL syntax", ex.Message, StringComparison.OrdinalIgnoreCase);
        }
    }
}
