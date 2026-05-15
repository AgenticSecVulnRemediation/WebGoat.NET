using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProvider_UpdateCustomerPassword_ParameterizedQueryTests
    {
        [Fact]
        public void UpdateCustomerPassword_SourceUsesParametersForPasswordAndCustomerNumber()
        {
            // Delta test for PR #636: UPDATE uses @password and @customerNumber parameters.
            var src = System.IO.File.ReadAllText("WebGoat/App_Code/DB/MySqlDbProvider.cs");

            Assert.Contains("update CustomerLogin set password = @password", src);
            Assert.Contains("where customerNumber = @customerNumber", src);
            Assert.Contains("Parameters.AddWithValue(\"@password\"", src);
            Assert.Contains("Parameters.AddWithValue(\"@customerNumber\"", src);
        }
    }
}
