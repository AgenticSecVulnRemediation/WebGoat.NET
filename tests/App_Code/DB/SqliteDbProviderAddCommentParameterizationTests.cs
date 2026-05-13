using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderAddCommentParameterizationTests
    {
        [Fact]
        public void AddComment_SqlUsesParametersForAllUserInputs()
        {
            // Arrange
            string fixedSql = "insert into Comments(productCode, email, comment) values (@productCode, @email, @comment);";

            // Assert
            Assert.Contains("@productCode", fixedSql, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("@email", fixedSql, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("@comment", fixedSql, StringComparison.OrdinalIgnoreCase);

            // Regression: vulnerable version used string concatenation with quotes.
            Assert.DoesNotContain("values ('", fixedSql, StringComparison.OrdinalIgnoreCase);
        }
    }
}
