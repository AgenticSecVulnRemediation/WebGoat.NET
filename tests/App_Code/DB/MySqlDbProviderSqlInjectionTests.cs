using System;
using System.IO;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderSqlInjectionTests
    {
        [Fact]
        public void GetCustomerEmailsQuery_UsesParameterInsteadOfConcatenation()
        {
            // Arrange
            var sourcePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "WebGoat", "App_Code", "DB", "MySqlDbProvider.cs");
            if (!File.Exists(sourcePath))
            {
                sourcePath = Path.Combine(Directory.GetCurrentDirectory(), "WebGoat", "App_Code", "DB", "MySqlDbProvider.cs");
            }

            // Act
            string content = File.ReadAllText(sourcePath);

            // Assert
            Assert.Contains("where email like @email", content);
            Assert.Contains("AddWithValue(\"@email\"", content);
            Assert.DoesNotContain("where email like '\" + email", content);
        }

        [Fact]
        public void GetProductsAndCategories_WhenFiltering_UsesCatNumberParameter()
        {
            // Arrange
            var sourcePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "WebGoat", "App_Code", "DB", "MySqlDbProvider.cs");
            if (!File.Exists(sourcePath))
            {
                sourcePath = Path.Combine(Directory.GetCurrentDirectory(), "WebGoat", "App_Code", "DB", "MySqlDbProvider.cs");
            }

            // Act
            string content = File.ReadAllText(sourcePath);

            // Assert
            Assert.Contains("select * from Categories where catNumber = @catNumber", content);
            Assert.Contains("select * from Products where catNumber = @catNumber", content);
            Assert.DoesNotContain("catClause += \" where catNumber = \" + catNumber", content);
        }
    }
}
