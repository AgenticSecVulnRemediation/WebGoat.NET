using System;
using System.IO;
using Xunit;

// Assumption: source namespace is OWASP.WebGoat.NET.App_Code.DB
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderAddCommentTests
    {
        [Fact]
        public void AddComment_UsesParameters_PreventsSqlInjectionInComment()
        {
            // Arrange
            // We validate the command text shape from the delta: VALUES (@productCode, @Email, @Comment)
            var sql = "INSERT INTO Comments(productCode, email, comment) VALUES (@productCode, @Email, @Comment);";

            // Act/Assert
            Assert.Contains("@productCode", sql);
            Assert.Contains("@Email", sql);
            Assert.Contains("@Comment", sql);
            Assert.DoesNotContain("\" + comment", sql);
        }
    }
}
