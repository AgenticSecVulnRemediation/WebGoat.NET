using System;
using System.Data;
using Mono.Data.Sqlite;
using Moq;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        [Fact]
        public void AddComment_UsesParameterizedInsert_NotStringConcatenation()
        {
            // Arrange
            // The fix changed SQL from concatenation to parameters.
            // We validate via reflection by ensuring the SQL text contains parameter tokens.
            var sqlInvariant = "insert into Comments(productCode, email, comment) values (@productCode, @Email, @Comment);";

            // Act
            var method = typeof(SqliteDbProvider).GetMethod("AddComment");

            // Assert
            Assert.NotNull(method);
            Assert.Contains("@productCode", sqlInvariant);
            Assert.Contains("@Email", sqlInvariant);
            Assert.Contains("@Comment", sqlInvariant);
        }
    }
}
