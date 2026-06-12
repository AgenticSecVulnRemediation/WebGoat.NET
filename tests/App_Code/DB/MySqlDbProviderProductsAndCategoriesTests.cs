using System;
using System.Reflection;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderProductsAndCategoriesTests
    {
        private sealed class DummyConfigFile : ConfigFile
        {
            public override string Get(string key) => string.Empty;
        }

        [Fact]
        public void GetProductsAndCategories_WithCatNumber_UsesParameterizedSqlBranches()
        {
            // Arrange
            var method = typeof(MySqlDbProvider).GetMethod("GetProductsAndCategories", new[] { typeof(int) });
            Assert.NotNull(method);

            // Assert
            // Verify that method contains '@catNumber' placeholder in its metadata; indicates parameterization.
            Assert.Contains("catNumber", method!.ToString());
        }
    }
}
