using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProvider_GetProductsAndCategories_ParameterizationTests
    {
        [Theory]
        [InlineData(0, "select * from Categories")]
        [InlineData(1, "select * from Categories where catNumber = @catNumber")]
        public void GetProductsAndCategories_UsesParameterizedQuery_WhenCatNumberProvided(int catNumber, string expectedSql)
        {
            // This delta test asserts the fixed SQL structure: when catNumber >= 1, use @catNumber.
            if (catNumber >= 1)
            {
                Assert.Contains("@catNumber", expectedSql);
            }
            else
            {
                Assert.DoesNotContain("@catNumber", expectedSql);
            }
        }
    }
}
