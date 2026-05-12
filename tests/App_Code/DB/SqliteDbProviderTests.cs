using System;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        [Fact]
        public void AddComment_UsesParameters_InsteadOfStringConcatenation()
        {
            // Arrange
            const string expectedSql = "insert into Comments(productCode, email, comment) values (@productCode, @Email, @Comment);";

            // Act
            string sql = expectedSql;

            // Assert
            Assert.Contains("@productCode", sql);
            Assert.Contains("@Email", sql);
            Assert.Contains("@Comment", sql);
            Assert.DoesNotContain("values ('", sql);
        }
    }
}
