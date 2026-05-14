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
        public void AddComment_AllowsQuotesInInputs_WithoutThrowing_FromSqlConcatenation()
        {
            // Delta test for fix: SQL concatenation -> parameterized insert.
            // With parameters, quotes in inputs should not break SQL syntax.

            // Arrange
            var config = new Mock<ConfigFile>();
            config.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            var provider = new MySqlDbProvider(config.Object);

            string productCode = "S10_1678";
            string email = "test'@example.com";
            string comment = "Nice'); DROP TABLE Comments;--";

            // Act
            var ex = Record.Exception(() => provider.AddComment(productCode, email, comment));

            // Assert
            Assert.Null(ex);
        }
    }
}
