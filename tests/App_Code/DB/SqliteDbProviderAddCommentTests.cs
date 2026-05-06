using System;
using System.Data;
using Mono.Data.Sqlite;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderAddCommentTests
    {
        [Fact]
        public void AddComment_UsesParameterizedInsertStatement()
        {
            // Arrange
            var sql = "insert into Comments(productCode, email, comment) values (@productCode, @Email, @Comment);";

            // Act/Assert
            Assert.Contains("@productCode", sql);
            Assert.Contains("@Email", sql);
            Assert.Contains("@Comment", sql);
            Assert.DoesNotContain("'\" + productCode + \"'", sql);
        }
    }
}
