using System;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderAddCommentTests
    {
        [Fact]
        public void AddComment_UsesParameterizedInsert_TemplateDoesNotContainUserInput()
        {
            // Arrange
            var productCode = "p' ; DROP TABLE Comments;--";
            var email = "e' OR '1'='1";
            var comment = "c'); DELETE FROM Comments;--";

            // Act/Assert
            const string sql = "insert into Comments(productCode, email, comment) values (@productCode, @email, @comment);";
            Assert.Contains("@productCode", sql);
            Assert.Contains("@email", sql);
            Assert.Contains("@comment", sql);
            Assert.DoesNotContain(productCode, sql);
            Assert.DoesNotContain(email, sql);
            Assert.DoesNotContain(comment, sql);
        }
    }
}
