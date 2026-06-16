using System;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderAddCommentParameterizationTests
    {
        [Fact]
        public void AddComment_UsesParameterizedInsert_DoesNotConcatenateValues()
        {
            // Arrange
            var method = typeof(SqliteDbProvider).GetMethod("AddComment");
            Assert.NotNull(method);

            // Assert
            var text = System.IO.File.ReadAllText(method!.Module.FullyQualifiedName);
            Assert.Contains("values (@productCode, @email, @comment)", text);
            Assert.Contains("Parameters.AddWithValue(\"@productCode\"", text);
            Assert.Contains("Parameters.AddWithValue(\"@email\"", text);
            Assert.Contains("Parameters.AddWithValue(\"@comment\"", text);
        }
    }
}
