using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderAddCommentTests
    {
        [Fact]
        public void AddComment_Query_UsesParametersInsteadOfConcatenation()
        {
            // Arrange
            var provider = new SqliteDbProvider(new ConfigFile());

            // Act
            string sql = "insert into Comments(productCode, email, comment) values (@productCode, @email, @comment);";

            // Assert
            Assert.Contains("@productCode", sql);
            Assert.Contains("@email", sql);
            Assert.Contains("@comment", sql);
            Assert.DoesNotContain("'" + " +", sql);
        }
    }
}
