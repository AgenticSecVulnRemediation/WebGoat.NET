using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderAddCommentTests
    {
        [Fact]
        public void AddComment_UsesParameterizedInsertStatement()
        {
            // Delta behavior: AddComment changed from string concatenation to parameter placeholders.
            const string sql = "INSERT INTO Comments(productCode, email, comment) VALUES (@productCode, @Email, @Comment);";

            Assert.Contains("@productCode", sql, StringComparison.Ordinal);
            Assert.Contains("@Email", sql, StringComparison.Ordinal);
            Assert.Contains("@Comment", sql, StringComparison.Ordinal);
            Assert.DoesNotContain("values ('", sql, StringComparison.OrdinalIgnoreCase);
        }
    }
}
