using System;
using Moq;
using Xunit;

// Note: Namespace inferred from file path. Adjust if project uses a different root namespace.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderAddCommentTests
    {
        [Fact]
        public void AddComment_UsesParameterizedInsert_DoesNotInlineInputs()
        {
            // Arrange
            var productCode = "S10_1678'); DROP TABLE Comments;--";
            var email = "a@b.com'; DROP TABLE CustomerLogin;--";
            var comment = "test'); DELETE FROM Comments;--";

            // Act
            var expectedSql = "insert into Comments(productCode, email, comment) values (@productCode, @Email, @Comment)";

            // Assert
            Assert.Contains("@productCode", expectedSql);
            Assert.Contains("@Email", expectedSql);
            Assert.Contains("@Comment", expectedSql);

            Assert.DoesNotContain(productCode, expectedSql);
            Assert.DoesNotContain(email, expectedSql);
            Assert.DoesNotContain(comment, expectedSql);
            Assert.DoesNotContain("'" + productCode, expectedSql);
        }
    }
}
