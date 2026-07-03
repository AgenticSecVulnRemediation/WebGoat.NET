using System;
using System.Linq;
using System.Reflection;
using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetOrdersTests
    {
        [Fact]
        public void GetOrders_UsesParameterizedCustomerId()
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
            Assert.Contains(literals, s => s.Contains("select * from Orders where customerNumber = @customerID", StringComparison.Ordinal));
            Assert.DoesNotContain(literals, s => s.Contains("select * from Orders where customerNumber = \" + customerID", StringComparison.Ordinal));
        }
    }
}
