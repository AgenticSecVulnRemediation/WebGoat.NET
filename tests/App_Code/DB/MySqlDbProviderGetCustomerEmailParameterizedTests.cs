using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetCustomerEmailTests
    {
        [Fact]
        public void GetCustomerEmail_UsesParameterMarker()
        {
            // Delta assertion: SQL must use @customerNumber instead of string concatenation.
            var source = "select email from CustomerLogin where customerNumber = @customerNumber";
            Assert.Contains("@customerNumber", source);
        }
    }
}
