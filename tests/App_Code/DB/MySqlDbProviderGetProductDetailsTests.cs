using System;
using System.Linq;
using System.Reflection;
using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedProductCodeQueries()
        {
            // Arrange
            var type = typeof(MySqlDbProvider);

            // Act
            var literals = type
                .GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance)
                .Select(f => f.GetRawConstantValue())
                .OfType<string>()
                .ToList();

            // Assert
            Assert.Contains(literals, s => s.Contains("select * from Products where productCode = @productCode", StringComparison.Ordinal));
            Assert.Contains(literals, s => s.Contains("select * from Comments where productCode = @productCode", StringComparison.Ordinal));

            // Previous vulnerable pattern used string concatenation with quotes.
            Assert.DoesNotContain(literals, s => s.Contains("select * from Products where productCode = '\" + productCode", StringComparison.Ordinal));
        }
    }
}
