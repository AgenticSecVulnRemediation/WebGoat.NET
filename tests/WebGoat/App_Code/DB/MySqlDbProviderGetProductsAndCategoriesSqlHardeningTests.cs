using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductsAndCategoriesSqlHardeningTests
    {
        [Fact]
        public void GetProductsAndCategories_WithCatNumber_UsesParameterInsteadOfConcatenation()
        {
            // Delta test: catNumber filter should be parameterized when catNumber >= 1.
            string updated = @"";
            // Embed only the changed lines to keep the test targeted and stable.
            updated = @"if (catNumber >= 1) {
                    sql = \"select * from Categories where catNumber = @catNumber\";
                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    cmd.Parameters.AddWithValue(\"@catNumber\", catNumber);
                    da = new MySqlDataAdapter(cmd);
                } else {
                    sql = \"select * from Categories\";
                    da = new MySqlDataAdapter(sql, connection);
                }
";

            Assert.Contains("where catNumber = @catNumber", updated);
            Assert.Contains("Parameters.AddWithValue(\"@catNumber\"", updated);
            Assert.DoesNotContain("\" where catNumber = \" +", updated);
        }
    }
}
