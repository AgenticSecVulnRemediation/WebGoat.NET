using System;
using System.IO;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProvider_AddCommentParameterizationTests
    {
        [Fact]
        public void AddComment_UsesParameterizedInsert_NotConcatenatedValues()
        {
            // Delta test: SQL changed from string-concatenated values to parameterized values.
            // We assert on source text to precisely lock the new secure behavior.

            var path = Path.Combine(Directory.GetCurrentDirectory(), "WebGoat", "App_Code", "DB", "MySqlDbProvider.cs");
            Assert.True(File.Exists(path), $"Expected file at {path}");

            var text = File.ReadAllText(path);

            Assert.Contains("insert into Comments(productCode, email, comment) values (@productCode, @email, @comment)", text);
            Assert.Contains("command.Parameters.AddWithValue(\"@productCode\"", text);
            Assert.Contains("command.Parameters.AddWithValue(\"@email\"", text);
            Assert.Contains("command.Parameters.AddWithValue(\"@comment\"", text);

            // Old vulnerable pattern should not exist.
            Assert.DoesNotContain("values ('\" + productCode", text);
            Assert.DoesNotContain("'\" + email", text);
            Assert.DoesNotContain("'\" + comment", text);
        }
    }
}
