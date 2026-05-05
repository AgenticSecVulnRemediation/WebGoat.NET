using System;
using System.Reflection;
using Moq;
using Xunit;

// Assumption: production namespace matches source file.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedQuery_ForProductCode()
        {
            // Arrange
            // Security fix: productCode was inlined into SQL; now it must be parameterized as @productCode.
            var cfg = new Mock<ConfigFile>();
            cfg.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);

            var provider = new MySqlDbProvider(cfg.Object);

            // Act
            var method = typeof(MySqlDbProvider).GetMethod("GetProductDetails", BindingFlags.Public | BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
            Assert.True(ContainsUserString(method!.Module, "select * from Products where productCode = @productCode"),
                "Expected Products query to use @productCode parameter");
            Assert.True(ContainsUserString(method.Module, "select * from Comments where productCode = @productCode"),
                "Expected Comments query to use @productCode parameter");
        }

        private static bool ContainsUserString(Module module, string expected)
        {
            try
            {
                var location = module.Assembly.Location;
                if (string.IsNullOrEmpty(location))
                    return false;

                var bytes = System.IO.File.ReadAllBytes(location);
                var text = System.Text.Encoding.UTF8.GetString(bytes);
                return text.Contains(expected, StringComparison.Ordinal);
            }
            catch
            {
                return false;
            }
        }
    }
}
