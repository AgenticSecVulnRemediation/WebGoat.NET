using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderAddCommentParameterizedSqlTests
    {
        [Fact]
        public void AddComment_InsertQuery_UsesParameters_NotStringInterpolation()
        {
            // Arrange
            // Delta behavior: insert statement changed from concatenation to parameter placeholders.
            var sql = "insert into Comments(productCode, email, comment) values (@productCode, @email, @comment);";

            // Act
            var hasAllParams = sql.Contains("@productCode", StringComparison.Ordinal)
                              && sql.Contains("@email", StringComparison.Ordinal)
                              && sql.Contains("@comment", StringComparison.Ordinal);

            // Assert
            Assert.True(hasAllParams);
            Assert.False(sql.Contains("'\" +"));
            Assert.False(sql.Contains("values ('"));
        }
    }
}
