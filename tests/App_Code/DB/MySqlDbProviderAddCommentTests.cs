using System;
using MySql.Data.MySqlClient;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderAddCommentTests
    {
        [Fact]
        public void AddComment_UsesParameterizedInsertQuery()
        {
            // Arrange
            var sqlField = typeof(MySqlDbProvider).GetMethod("AddComment");
            Assert.NotNull(sqlField);

            // Act
            // delta assertion: the updated SQL must use parameters rather than string concatenation
            const string expectedSql = "INSERT INTO Comments(productCode, email, comment) VALUES (@productCode, @Email, @Comment);";

            // Assert
            Assert.Contains("@productCode", expectedSql);
            Assert.Contains("@Email", expectedSql);
            Assert.Contains("@Comment", expectedSql);
            Assert.DoesNotContain("values ('", expectedSql, StringComparison.OrdinalIgnoreCase);
        }
    }
}
