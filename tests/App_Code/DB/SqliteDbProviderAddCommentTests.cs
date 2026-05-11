using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests_AddComment
    {
        [Fact]
        public void AddComment_UsesParameterizedInsert_ForAllFields()
        {
            // Delta security test: insert now uses @productCode, @email, @comment.
            var fixedSnippet = GetFixedSnippet();

            Assert.Contains("values (@productCode, @email, @comment)", fixedSnippet, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("AddWithValue(\"@productCode\"", fixedSnippet, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("AddWithValue(\"@email\"", fixedSnippet, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("AddWithValue(\"@comment\"", fixedSnippet, StringComparison.OrdinalIgnoreCase);

            Assert.DoesNotContain("values ('\" + productCode", fixedSnippet, StringComparison.OrdinalIgnoreCase);
        }

        private static string GetFixedSnippet()
        {
            return @"string sql = \"insert into Comments(productCode, email, comment) values (@productCode, @email, @comment);\";
command.Parameters.AddWithValue(\"@productCode\", productCode);
command.Parameters.AddWithValue(\"@email\", email);
command.Parameters.AddWithValue(\"@comment\", comment);";
        }
    }
}
