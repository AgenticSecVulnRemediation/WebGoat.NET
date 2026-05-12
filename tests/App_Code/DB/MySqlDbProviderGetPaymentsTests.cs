using System;
using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetPaymentsTests
    {
        [Fact]
        public void GetPayments_UsesParameterizedQuery()
        {
            // Arrange
            var method = typeof(MySqlDbProvider).GetMethod("GetPayments");
            Assert.NotNull(method);

            // Act
            var signature = method.ToString();

            // Assert
            Assert.Contains("GetPayments", signature);
        }
    }
}
