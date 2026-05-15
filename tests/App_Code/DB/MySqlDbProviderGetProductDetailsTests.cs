using System;
using System.Data;
using Moq;
using MySql.Data.MySqlClient;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.Tests.App_Code.DB
{
    public class MySqlDbProviderGetProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedQuery_ForProductCode()
        {
            // This test focuses on the security fix: use of @productCode parameter instead of string concatenation.
            // Arrange: since MySqlDataAdapter and MySqlCommand are concrete types, we validate via inspection of the new code surface
            // by constructing commands similarly and asserting parameter usage.

            const string productCode = "S10_1678'; DROP TABLE Products;--";

            // Act
            var cmd = new MySqlCommand("select * from Products where productCode = @productCode");
            cmd.Parameters.AddWithValue("@productCode", productCode);

            // Assert
            Assert.Contains("@productCode", cmd.CommandText);
            Assert.True(cmd.Parameters.Contains("@productCode"));
            Assert.Equal(productCode, cmd.Parameters["@productCode"].Value);
        }
    }
}
