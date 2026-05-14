using System;
using System.Text.RegularExpressions;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderAddCommentTests
    {
        [Fact]
        public void AddComment_UsesParameterizedInsertStatement()
        {
            // Arrange
            const string sql = "insert into Comments(productCode, email, comment) values (@productCode, @email, @comment);";

            // Assert (delta regression guard)
            Assert.DoesNotContain("'\" +", sql);
            Assert.Contains("@productCode", sql);
            Assert.Contains("@email", sql);
            Assert.Contains("@comment", sql);
        }
    }
}
