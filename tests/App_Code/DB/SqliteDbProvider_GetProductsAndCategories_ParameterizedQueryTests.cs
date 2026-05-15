using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_GetProductsAndCategories_ParameterizedQueryTests
    {
        [Fact]
        public void GetProductsAndCategories_SourceUsesCatNumberParameterWhenFiltering()
        {
            // Delta test for PR #639: SQLite provider now parameterizes catNumber.
            var src = System.IO.File.ReadAllText("WebGoat/App_Code/DB/SqliteDbProvider.cs");

            Assert.Contains("where catNumber = @catNumber", src);
            Assert.Contains("Parameters.AddWithValue(\"@catNumber\"", src);
            Assert.DoesNotContain("where catNumber = \" + catNumber", src);
        }
    }
}
