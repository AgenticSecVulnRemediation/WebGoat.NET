using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProvider_GetProductsAndCategories_ParameterizedQueryTests
    {
        [Fact]
        public void GetProductsAndCategories_SourceUsesCatNumberParameterWhenFiltering()
        {
            // Delta test for PR #633: catNumber filtering now uses @catNumber parameter.
            var src = System.IO.File.ReadAllText("WebGoat/App_Code/DB/MySqlDbProvider.cs");

            Assert.Contains("where catNumber = @catNumber", src);
            Assert.Contains("AddWithValue(\"@catNumber\"", src);
            Assert.DoesNotContain("where catNumber = \" + catNumber", src);
        }
    }
}
