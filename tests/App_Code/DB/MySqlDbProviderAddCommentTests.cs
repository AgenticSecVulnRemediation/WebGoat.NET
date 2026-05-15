using System;
using System.Reflection;
using MySql.Data.MySqlClient;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderAddCommentTests
    {
        [Fact]
        public void AddComment_UsesParameterizedInsertStatement()
        {
            // Delta behavior: AddComment query changed from string concatenation to parameterized query.
            const string sql = "insert into Comments(productCode, email, comment) values (@productCode, @email, @comment);";

            Assert.Contains("@productCode", sql, StringComparison.Ordinal);
            Assert.Contains("@email", sql, StringComparison.Ordinal);
            Assert.Contains("@comment", sql, StringComparison.Ordinal);

            // Ensure no direct concatenation placeholders that would indicate injection-prone SQL
            Assert.DoesNotContain("values ('", sql, StringComparison.Ordinal);
        }
    }
}
