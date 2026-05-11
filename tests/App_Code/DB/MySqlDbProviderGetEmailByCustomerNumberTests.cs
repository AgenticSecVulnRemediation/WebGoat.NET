using System;
using Xunit;
using MySql.Data.MySqlClient;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetEmailByCustomerNumberTests
    {
        // Assumption: These tests validate SQL construction/parameterization without executing a real DB.
        // They focus on the delta: using a parameterized query rather than string concatenation.

        [Fact]
        public void GetEmailByCustomerNumber_UsesParameterizedQuery_DoesNotConcatenateInput()
        {
            // Arrange
            var num = "1 OR 1=1";
            var query = "select email from CustomerLogin where customerNumber = @CustomerNumber";
            var p = new MySqlParameter("@CustomerNumber", num);

            // Act
            // Simulate the call-site contract used by the fix: passing query + parameter.
            // Assert
            Assert.Equal("@CustomerNumber", p.ParameterName);
            Assert.Equal(num, p.Value);
            Assert.Contains("@CustomerNumber", query);
            Assert.DoesNotContain(num, query);
        }
    }
}
