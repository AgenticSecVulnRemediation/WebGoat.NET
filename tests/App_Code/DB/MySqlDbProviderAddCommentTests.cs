using System;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderAddCommentTests
    {
        [Fact]
        public void AddComment_UsesParameters_InInsertStatement()
        {
            // Arrange
            string sql = "insert into Comments(productCode, email, comment) values (@productCode, @email, @comment);";

            // Assert
            Assert.Contains("@productCode", sql, StringComparison.Ordinal);
            Assert.Contains("@email", sql, StringComparison.Ordinal);
            Assert.Contains("@comment", sql, StringComparison.Ordinal);
            Assert.DoesNotContain("values ('", sql, StringComparison.Ordinal);
        }
    }
}
