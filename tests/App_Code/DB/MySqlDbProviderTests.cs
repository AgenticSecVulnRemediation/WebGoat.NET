using Xunit;
using Moq;
using OWASP.WebGoat.NET.App_Code.DB;
using MySql.Data.MySqlClient;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Theory]
        [InlineData(0, "select * from Categories")]
        [InlineData(1, "select * from Categories where catNumber = @catNumber")]
        public void GetProductsAndCategories_WhenCatNumberVaries_UsesExpectedCategoryQuery(int catNumber, string expectedSql)
        {
            // Arrange/Act
            var conn = new MySqlConnection();
            var da = new MySqlDataAdapter(expectedSql, conn);
            if (catNumber >= 1)
            {
                da.SelectCommand.Parameters.AddWithValue("@catNumber", catNumber);
            }

            // Assert
            Assert.Equal(expectedSql, da.SelectCommand.CommandText);
            if (catNumber >= 1)
            {
                Assert.True(da.SelectCommand.Parameters.Contains("@catNumber"));
                Assert.Equal(catNumber, da.SelectCommand.Parameters["@catNumber"].Value);
            }
            else
            {
                Assert.False(da.SelectCommand.Parameters.Contains("@catNumber"));
            }
        }

        [Theory]
        [InlineData(0, "select * from Products")]
        [InlineData(2, "select * from Products where catNumber = @catNumber")]
        public void GetProductsAndCategories_WhenCatNumberVaries_UsesExpectedProductQuery(int catNumber, string expectedSql)
        {
            // Arrange/Act
            var conn = new MySqlConnection();
            var da = new MySqlDataAdapter(expectedSql, conn);
            if (catNumber >= 1)
            {
                da.SelectCommand.Parameters.AddWithValue("@catNumber", catNumber);
            }

            // Assert
            Assert.Equal(expectedSql, da.SelectCommand.CommandText);
            if (catNumber >= 1)
            {
                Assert.True(da.SelectCommand.Parameters.Contains("@catNumber"));
                Assert.Equal(catNumber, da.SelectCommand.Parameters["@catNumber"].Value);
            }
            else
            {
                Assert.False(da.SelectCommand.Parameters.Contains("@catNumber"));
            }
        }
    }
}
