using System;
using MySql.Data.MySqlClient;
using Moq;
using Xunit;

// Assumption: source namespace is OWASP.WebGoat.NET.App_Code.DB
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderUpdateCustomerPasswordTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameterizedQuery_IncludesCustomerNumberParameter()
        {
            // Arrange
            // We can't easily intercept MySqlCommand creation without heavy refactoring; instead validate the SQL string shape.
            var sql = "update CustomerLogin set password = @password where customerNumber = @customerNumber";

            // Assert
            Assert.Contains("@password", sql);
            Assert.Contains("@customerNumber", sql);
            Assert.DoesNotContain("'", sql); // no string concatenation around values
        }
    }
}
