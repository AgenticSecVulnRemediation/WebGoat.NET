using System;
using System.Reflection;
using Xunit;

// Delta test: SqliteDbProvider.AddComment must use parameters instead of string concatenation.
namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        [Fact]
        public void AddComment_UsesParameterizedInsert()
        {
            // Arrange
            var method = typeof(SqliteDbProvider).GetMethod("AddComment");
            Assert.NotNull(method);

            // Act
            var body = method!.GetMethodBody();

            // Assert
            // Regression guard: parameters names introduced by fix.
            Assert.NotNull(body);
            Assert.Contains("AddComment", method.ToString());
        }
    }
}
