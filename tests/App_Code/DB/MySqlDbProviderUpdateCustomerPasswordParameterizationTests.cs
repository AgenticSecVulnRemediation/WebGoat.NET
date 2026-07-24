using System;
using Moq;
using MySql.Data.MySqlClient;
using OWASP.WebGoat.NET.App_Code;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProvider_UpdateCustomerPassword_UsesParameters_Tests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameterizedQueryAndDoesNotConcatenateInputs()
        {
            // Arrange: enforce delta via source assertion to avoid DB dependency
            var source = System.IO.File.ReadAllText("WebGoat/App_Code/DB/MySqlDbProvider.cs");

            // Assert: new SQL uses parameters
            Assert.Contains("SET password = @password", source);
            Assert.Contains("WHERE customerNumber = @customerNumber", source);
            Assert.Contains("AddWithValue(\"@password\"", source);
            Assert.Contains("AddWithValue(\"@customerNumber\"", source);

            // Assert: old vulnerable pattern removed
            Assert.DoesNotContain("update CustomerLogin set password = '\" +", source);
        }
    }
}
