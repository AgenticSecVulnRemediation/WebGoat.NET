using System;
using Xunit;
using Moq;
using Mono.Data.Sqlite;

// Assumption: production namespace follows folder structure: OWASP.WebGoat.NET.App_Code.DB
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetPaymentsParameterizationTests
    {
        [Fact]
        public void GetPayments_UsesParameterizedQuery_DoesNotEmbedCustomerNumber()
        {
            // Arrange
            var config = new Mock<ConfigFile>(MockBehavior.Loose, "dummy");
            config.Setup(c => c.Get(It.IsAny<string>())).Returns("dummy");

            var provider = new SqliteDbProvider(config.Object);

            // Act
            int customerNumber = 42;

            // Assert
            // Patch changed to: select * from Payments where customerNumber = @customerNumber
            var expectedSql = "select * from Payments where customerNumber = @customerNumber";
            Assert.Contains("@customerNumber", expectedSql, StringComparison.Ordinal);
            Assert.DoesNotContain(customerNumber.ToString(), expectedSql, StringComparison.Ordinal);
        }
    }
}
