using System;
using System.Reflection;
using Xunit;

// Assumption: production namespace matches source file.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_UsesParameterizedQueries_ForProductCode()
        {
            // Arrange / Act
            var method = typeof(SqliteDbProvider).GetMethod("GetProductDetails", BindingFlags.Public | BindingFlags.Instance);

            // Assert
            Assert.NotNull(method);
            Assert.True(ContainsUserString(method!.Module, "select * from Products where productCode = @productCode"),
                "Expected Products query to be parameterized with @productCode");
            Assert.True(ContainsUserString(method.Module, "select * from Comments where productCode = @productCode"),
                "Expected Comments query to be parameterized with @productCode");
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
