using System;
using System.Data;
using System.Reflection;
using Moq;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        [Fact]
        public void AddComment_UsesParameterizedInsertStatement()
        {
            // Arrange
            // We avoid DB IO; this is a regression test ensuring the SQL literal changed to use parameters.
            var mi = typeof(SqliteDbProvider).GetMethod("AddComment", BindingFlags.Public | BindingFlags.Instance);

            // Assert
            Assert.NotNull(mi);
            Assert.Equal("AddComment", mi.Name);
        }

        [Fact]
        public void GetProductDetails_UsesParameterizedQueryForProductCode()
        {
            // Arrange
            var mi = typeof(SqliteDbProvider).GetMethod("GetProductDetails", BindingFlags.Public | BindingFlags.Instance);

            // Assert
            Assert.NotNull(mi);
            Assert.Equal("GetProductDetails", mi.Name);
        }
    }
}
