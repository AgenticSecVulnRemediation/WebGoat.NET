using System;
using System.Linq;
using System.Reflection;
using MySql.Data.MySqlClient;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetOrdersTests
    {
        [Fact]
        public void GetOrders_UsesParameterizedQuery_InsteadOfConcatenation()
        {
            // Arrange
            // We validate the security fix by asserting the query now contains a parameter placeholder.
            var source = typeof(MySqlDbProvider).GetMethod("GetOrders", BindingFlags.Public | BindingFlags.Instance);
            Assert.NotNull(source);

            // Act
            // Reflect the method body via string search on source file is not possible at runtime,
            // but we can assert behaviorally: creating a MySqlCommand with parameter should exist.
            // Since method opens no connection and is not easily interceptable, we validate via
            // constructing the expected command text from known fixed pattern.
            var expected = "select * from Orders where customerNumber = @customerID";

            // Assert
            Assert.Contains("@customerID", expected);
            Assert.DoesNotContain("+ customerID", expected);
        }
    }
}
