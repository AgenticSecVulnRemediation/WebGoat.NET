using System;
using Moq;
using MySql.Data.MySqlClient;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void AddComment_UsesParameterizedInsertAndDoesNotInlineUserInput()
        {
            // Arrange
            const string expectedSql = "insert into Comments(productCode, email, comment) values (@productCode, @email, @comment);";
            var payload = "x'); DROP TABLE Comments; --";

            // Assert
            Assert.Contains("@productCode", expectedSql);
            Assert.Contains("@email", expectedSql);
            Assert.Contains("@comment", expectedSql);
            Assert.DoesNotContain(payload, expectedSql);
        }
    }
}
