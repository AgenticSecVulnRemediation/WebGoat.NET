using System;
using System.Reflection;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductDetailsTests
    {
        [Fact]
        public void GetProductDetails_IsPresent_AfterParameterizationChange()
        {
            // Arrange
            var method = typeof(SqliteDbProvider).GetMethod("GetProductDetails", BindingFlags.Instance | BindingFlags.Public);

            // Assert
            Assert.NotNull(method);
            Assert.NotNull(method!.GetMethodBody());
        }
    }
}
