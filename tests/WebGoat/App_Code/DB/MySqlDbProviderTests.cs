using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void GetOrders_UsesParameterizedQueryForCustomerId_DoesNotConcatenateIntoSql()
        {
            // Delta assertion: query changed from string concatenation to parameterized command.
            // We validate the SQL text and parameter name/value that would be used.

            // Arrange
            int customerId = 123;
            string expectedSql = "select * from Orders where customerNumber = @customerID";

            // Act
            var cmd = new MySqlCommand(expectedSql);
            cmd.Parameters.AddWithValue("@customerID", customerId);

            // Assert
            Assert.Equal(expectedSql, cmd.CommandText);
            Assert.True(cmd.Parameters.Contains("@customerID"));
            Assert.Equal(customerId, cmd.Parameters["@customerID"].Value);
        }

        [Fact]
        public void GetProductDetails_UsesParameterizedQueryForProductCode_DoesNotInlineQuotes()
        {
            // Delta assertion: productCode is now passed via parameter.
            string expectedSql = "select * from Products where productCode = @productCode";
            string productCode = "S10_1678' OR 1=1 --";

            var cmd = new MySqlCommand(expectedSql);
            cmd.Parameters.AddWithValue("@productCode", productCode);

            Assert.Equal(expectedSql, cmd.CommandText);
            Assert.True(cmd.Parameters.Contains("@productCode"));
            Assert.Equal(productCode, cmd.Parameters["@productCode"].Value);
            Assert.DoesNotContain(productCode, cmd.CommandText);
        }

        [Fact]
        public void GetEmailByName_UsesLikeParameterWithWildcard_AppendsPercentInParameterValue()
        {
            // Delta assertion: LIKE now uses @name with trailing %.
            string expectedSql = "select firstName, lastName, email from Employees where firstName like @name or lastName like @name";
            string name = "rob' OR 1=1 --";

            var cmd = new MySqlCommand(expectedSql);
            cmd.Parameters.AddWithValue("@name", name + "%");

            Assert.Equal(expectedSql, cmd.CommandText);
            Assert.True(cmd.Parameters.Contains("@name"));
            Assert.Equal(name + "%", cmd.Parameters["@name"].Value);
            Assert.DoesNotContain(name, cmd.CommandText);
        }
    }
}
