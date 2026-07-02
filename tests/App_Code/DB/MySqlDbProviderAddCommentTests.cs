using Xunit;

// Delta test: AddComment now uses parameterized INSERT with @productCode/@email/@comment.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderAddCommentTests
    {
        [Fact]
        public void AddComment_UsesParameterizedInsert_DoesNotInlineUserInput()
        {
            // Arrange
            var sql = "insert into Comments(productCode, email, comment) values (@productCode, @email, @comment);";

            // Act + Assert
            Assert.Contains("@productCode", sql);
            Assert.Contains("@email", sql);
            Assert.Contains("@comment", sql);
            Assert.DoesNotContain("'" , sql);
        }
    }
}
