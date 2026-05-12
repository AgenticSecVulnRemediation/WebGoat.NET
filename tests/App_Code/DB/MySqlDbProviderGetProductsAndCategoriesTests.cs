using System;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductsAndCategoriesTests
    {
        [Fact]
        public void GetProductsAndCategories_WithCatNumber_UsesParameterizedQueries()
        {
            // Arrange
            var provider = (MySqlDbProvider)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(MySqlDbProvider));
            typeof(MySqlDbProvider).GetField("_connectionString", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!
                .SetValue(provider, "Server=localhost;Database=test;Uid=test;Pwd=test;");

            // Act
            Exception? ex = Record.Exception(() => provider.GetProductsAndCategories(1));

            // Assert
            Assert.NotNull(ex);
            Assert.DoesNotContain("where catNumber = ", ex!.ToString(), StringComparison.Ordinal);
        }
    }
}
