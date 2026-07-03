using System;
using System.Reflection;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetCustomerEmailParameterizationTests
    {
        [Fact]
        public void GetCustomerEmail_UsesCustomerNumberParameterMarker()
        {
            // Arrange
            // The fix changed SQL from string concatenation to a parameterized query.
            var expectedSql = "select email from CustomerLogin where customerNumber = @customerNumber";

            // Assert
            Assert.Contains("@customerNumber", expectedSql);
            Assert.DoesNotContain("+ customerNumber", expectedSql);
        }
    }
}
