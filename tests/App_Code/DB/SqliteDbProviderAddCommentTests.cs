using System;
using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderAddCommentTests
    {
        [Fact]
        public void AddComment_UsesParameterizedInsert_NotStringConcatenation()
        {
            // Arrange
            var provider = (SqliteDbProvider)Activator.CreateInstance(typeof(SqliteDbProvider), args: new object[] { null });

            // Act
            var moduleBytes = System.IO.File.ReadAllBytes(typeof(SqliteDbProvider).Module.FullyQualifiedName);
            var moduleText = System.Text.Encoding.UTF8.GetString(moduleBytes);

            // Assert
            Assert.Contains("insert into Comments(productCode, email, comment) values (@productCode, @email, @comment)", moduleText, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("values ('\" + productCode", moduleText, StringComparison.OrdinalIgnoreCase);
        }
    }
}
