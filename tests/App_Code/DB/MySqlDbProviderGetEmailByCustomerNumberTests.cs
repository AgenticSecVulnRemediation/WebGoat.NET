using System;
using System.Linq;
using System.Reflection;
using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetEmailByCustomerNumberTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_UsesParameterizedQuery()
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
            Assert.Contains(literals, s => s.Contains("SELECT email FROM CustomerLogin WHERE customerNumber = @customerNumber", StringComparison.Ordinal));
            Assert.DoesNotContain(literals, s => s.Contains("select email from CustomerLogin where customerNumber = \" + num", StringComparison.OrdinalIgnoreCase));
        }
    }
}
