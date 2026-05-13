using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void AddComment_UsesParameterizedInsert_ForAllValues()
        {
            // Arrange
            const string sql = "insert into Comments(productCode, email, comment) values (@productCode, @email, @comment);";

            // Assert
            Assert.Contains("@productCode", sql);
            Assert.Contains("@email", sql);
            Assert.Contains("@comment", sql);
            Assert.DoesNotContain("'\" +", sql);
        }
    }
}
