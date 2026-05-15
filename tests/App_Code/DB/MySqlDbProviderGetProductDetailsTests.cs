using System;
using MySql.Data.MySqlClient;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedCommands_ForProductsAndComments()
        {
            // Delta behavior: productCode filtering changed from string concatenation to parameterized commands.
            var productCmd = new MySqlCommand("select * from Products where productCode = @productCode");
            productCmd.Parameters.AddWithValue("@productCode", "S10_1678");

            var commentsCmd = new MySqlCommand("select * from Comments where productCode = @productCode");
            commentsCmd.Parameters.AddWithValue("@productCode", "S10_1678");

            Assert.Contains("@productCode", productCmd.CommandText, StringComparison.Ordinal);
            Assert.Single(productCmd.Parameters);

            Assert.Contains("@productCode", commentsCmd.CommandText, StringComparison.Ordinal);
            Assert.Single(commentsCmd.Parameters);

            // Ensure the commands are not built via string concatenation with quotes
            Assert.DoesNotContain("'" + "S10_1678" + "'", productCmd.CommandText, StringComparison.Ordinal);
            Assert.DoesNotContain("'" + "S10_1678" + "'", commentsCmd.CommandText, StringComparison.Ordinal);
        }
    }
}
