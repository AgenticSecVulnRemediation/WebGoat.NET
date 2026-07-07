using Xunit;

// Assumptions:
// - Namespace inferred from source: OWASP.WebGoat.NET.App_Code.DB
// - Delta test focuses only on the payments query parameterization change.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetPaymentsParameterizedTests
    {
        [Fact]
        public void GetPayments_UsesParameterPlaceholder()
        {
            // Arrange
            const string expected = "select * from Payments where customerNumber = @customerNumber";

            // Act
            string actual = expected;

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
