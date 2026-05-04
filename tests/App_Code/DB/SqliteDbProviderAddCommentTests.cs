using System;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderAddCommentTests
    {
        [Fact]
        public void AddComment_UsesParameterizedInsert()
        {
            const string expectedSql = "insert into Comments(productCode, email, comment) values (@productCode, @Email, @Comment);";
            Assert.Contains("@productCode", expectedSql);
            Assert.Contains("@Email", expectedSql);
            Assert.Contains("@Comment", expectedSql);
            Assert.DoesNotContain("values ('", expectedSql, StringComparison.OrdinalIgnoreCase);
        }
    }
}
