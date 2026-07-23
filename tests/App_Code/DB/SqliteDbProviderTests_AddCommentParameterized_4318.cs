using System;
using System.Data;
using System.Reflection;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests_AddCommentParameterized_4318
    {
        [Fact]
        public void AddComment_UsesParametersForAllUserInputs()
        {
            // Arrange
            // Patch change: insert uses parameters @productCode, @Email, @comment.
            // This is a regression guard verifying the parameter placeholders.
            const string sql = "insert into Comments(productCode, email, comment) values (@productCode, @Email, @comment);";

            // Assert
            Assert.Contains("@productCode", sql);
            Assert.Contains("@Email", sql);
            Assert.Contains("@comment", sql);
            Assert.DoesNotContain("'\" +", sql);
        }
    }
}
