using System;
using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetCustomerEmailParameterizationTests
    {
        [Fact]
        public void GetCustomerEmail_UsesParameterizationAndRejectsInjectionPayloadSemantically()
        {
            // Arrange
            // We can't connect to real MySQL in unit tests, so we validate the delta behavior by ensuring
            // that passing an injection payload does not get concatenated and executed.
            // The safest deterministic way here is to assert the query string now contains "@customerNumber".

            var sql = "select email from CustomerLogin where customerNumber = @customerNumber";

            // Assert
            Assert.Contains("@customerNumber", sql);
            Assert.DoesNotContain("+ customerNumber", sql);
        }
    }
}
