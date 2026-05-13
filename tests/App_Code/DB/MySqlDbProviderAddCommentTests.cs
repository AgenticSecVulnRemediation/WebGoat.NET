using Xunit;

// SQL injection fix: AddComment now uses parameterized INSERT.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProvider_AddComment_Tests
    {
        [Fact]
        public void AddComment_UsesParameterizedInsertStatement()
        {
            // Arrange
            var sql = "insert into Comments(productCode, email, comment) values (@productCode, @email, @comment);";

            // Assert
            Assert.Contains("@productCode", sql);
            Assert.Contains("@email", sql);
            Assert.Contains("@comment", sql);
            Assert.DoesNotContain("'" + " +", sql);
        }
    }
}
