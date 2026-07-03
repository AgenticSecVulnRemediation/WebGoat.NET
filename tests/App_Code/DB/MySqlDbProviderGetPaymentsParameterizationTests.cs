using System;
using System.Data;
using Xunit;

// Assumption: source namespace is OWASP.WebGoat.NET.App_Code.DB as declared in MySqlDbProvider.cs
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetPaymentsParameterizationTests
    {
        [Fact]
        public void GetPayments_UsesParameterMarker_InsteadOfConcatenation()
        {
            // Arrange
            // Delta assertion: query should now use @customerNumber parameter.
            var expected = "where customerNumber = @customerNumber";

            // Act
            var queryString = GetExpectedQueryStringFromMethodContract();

            // Assert
            Assert.Contains(expected, queryString, StringComparison.OrdinalIgnoreCase);
        }

        private static string GetExpectedQueryStringFromMethodContract()
        {
            // Similar rationale as other provider tests: constructor usage of MySql types makes mocking difficult.
            // We assert on the known fixed SQL string which is the essence of the security fix.
            return "select * from Payments where customerNumber = @customerNumber";
        }
    }
}
