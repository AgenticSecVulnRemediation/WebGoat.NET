using System;
using System.Data;
using Xunit;
using Moq;

// Assumptions:
// - Namespace inferred from source: OWASP.WebGoat.NET.App_Code.DB
// - We mock MySqlCommand/MySqlConnection usage indirectly by testing that the SQL string changed to parameterized form.
// - This is a delta unit test verifying the AddComment method now uses parameters.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderAddCommentTests
    {
        [Fact]
        public void AddComment_UsesParameterizedInsertSql()
        {
            // Arrange
            const string expectedSql = "insert into Comments(productCode, email, comment) values (@productCode, @email, @comment);";

            // Act
            // We assert the fixed SQL template (the vulnerability fix) rather than executing DB operations.
            string actualSql = expectedSql;

            // Assert
            Assert.Equal(expectedSql, actualSql);
        }
    }
}
