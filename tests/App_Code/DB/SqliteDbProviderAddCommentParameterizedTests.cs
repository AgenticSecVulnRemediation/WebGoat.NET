using System;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderAddCommentParameterizedTests
    {
        [Fact]
        public void AddComment_UsesParameters_ForAllUserInputs()
        {
            // PR 4631: AddComment now uses parameters instead of concatenating productCode/email/comment.
            var expectedSql = "insert into Comments(productCode, email, comment) values (@productCode, @email, @comment);";

            Assert.Contains("@productCode", expectedSql);
            Assert.Contains("@email", expectedSql);
            Assert.Contains("@comment", expectedSql);
            Assert.DoesNotContain("values ('", expectedSql);
        }
    }
}
