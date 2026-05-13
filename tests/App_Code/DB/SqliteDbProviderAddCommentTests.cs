using Xunit;

namespace OWASP.WebGoat.NET.Tests.App_Code.DB
{
    public class SqliteDbProviderAddCommentTests
    {
        [Fact]
        public void AddComment_Query_IsParameterized()
        {
            // Arrange
            const string sql = "insert into Comments(productCode, email, comment) values (@productCode, @email, @comment);";

            // Assert
            Assert.Contains("@productCode", sql);
            Assert.Contains("@email", sql);
            Assert.Contains("@comment", sql);
            Assert.DoesNotContain("values ('", sql);
        }
    }
}
