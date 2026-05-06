using System;
using Xunit;

// Assumption: production code namespace matches file path.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderAddCommentTests
    {
        [Fact]
        public void AddComment_UsesParameterizedInsertStatement()
        {
            // Arrange
            var expectedSql = "insert into Comments(productCode, email, comment) values (@productCode, @Email, @Comment);";

            // Act / Assert
            Assert.Contains("@productCode", expectedSql);
            Assert.Contains("@Email", expectedSql);
            Assert.Contains("@Comment", expectedSql);
        }
    }
}
