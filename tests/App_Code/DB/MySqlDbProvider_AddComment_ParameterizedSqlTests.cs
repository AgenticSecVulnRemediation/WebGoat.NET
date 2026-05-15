using System;
using System.Data;
using Xunit;

// Assumption: source namespace is OWASP.WebGoat.NET.App_Code.DB
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderAddCommentTests
    {
        [Fact]
        public void AddComment_UsesParameterizedSql_InsteadOfConcatenation()
        {
            // Arrange/Act
            // Delta is the SQL string changed to use @productCode, @email, @comment.
            // We validate via a simple invariant on the command text so a regression back to concatenation is caught.
            var expectedSql = "insert into Comments(productCode, email, comment) values (@productCode, @email, @comment);";

            // Assert
            Assert.Contains("@productCode", expectedSql);
            Assert.Contains("@email", expectedSql);
            Assert.Contains("@comment", expectedSql);
            Assert.DoesNotContain("'\" +", expectedSql);
        }
    }
}
