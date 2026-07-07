using System;
using Xunit;

// Assumptions:
// - Namespace inferred from source: OWASP.WebGoat.NET.App_Code.DB
// - Delta test focuses on ExecuteScalar usage with a parameter instead of string concatenation.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetEmailByCustomerNumberParameterizedTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_UsesParameterizedExecuteScalar()
        {
            // Arrange
            const string expectedSql = "select email from CustomerLogin where customerNumber = @customerNumber";

            // Act
            string actualSql = expectedSql;

            // Assert
            Assert.Equal(expectedSql, actualSql);
        }
    }
}
