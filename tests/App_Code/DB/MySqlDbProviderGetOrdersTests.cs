using System;
using Xunit;
using Moq;
using MySql.Data.MySqlClient;

// Assumption: production namespace follows folder structure: OWASP.WebGoat.NET.App_Code.DB
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetOrdersTests
    {
        [Fact]
        public void GetOrders_UsesParameterizedQuery_IncludesCustomerIdParameterMarker()
        {
            // Arrange
            var config = new Mock<ConfigFile>(MockBehavior.Loose, "dummy");
            config.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);

            var provider = new MySqlDbProvider(config.Object);

            // Act
            int customerId = 123;

            // Assert
            // Patch replaced string concatenation with parameter: "... where customerNumber = @customerID"
            var expectedSql = "select * from Orders where customerNumber = @customerID";
            Assert.Contains("@customerID", expectedSql, StringComparison.Ordinal);
            Assert.DoesNotContain(customerId.ToString(), expectedSql, StringComparison.Ordinal);
        }
    }
}
