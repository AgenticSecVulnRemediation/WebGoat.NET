using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProvider_GetCustomerEmail_ParameterizedQueryTests
    {
        [Fact]
        public void GetCustomerEmail_SourceUsesCustomerNumberParameterPlaceholder()
        {
            // Delta test for PR #631: SQL now includes "customerNumber = @customerNumber".
            // Without DB connectivity, validate by reading the source file as text.
            var src = System.IO.File.ReadAllText("WebGoat/App_Code/DB/MySqlDbProvider.cs");
            Assert.Contains("customerNumber = @customerNumber", src);
            Assert.Contains("Parameters.AddWithValue(\"@customerNumber\"", src);
        }
    }
}
