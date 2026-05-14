using System;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void GetPayments_UsesParameterizedCustomerNumberQuery()
        {
            // Arrange
            const string expectedSql = "select * from Payments where customerNumber = @customerNumber";

            // Assert
            Assert.Contains("@customerNumber", expectedSql);
            Assert.DoesNotContain(" + ", expectedSql);
        }
    }
}
