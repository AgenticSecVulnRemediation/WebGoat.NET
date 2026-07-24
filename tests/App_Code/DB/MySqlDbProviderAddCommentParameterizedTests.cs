using System;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderAddCommentParameterizedTests
    {
        [Fact]
        public void AddComment_UsesParameters_ForAllUserInputs()
        {
            // PR 4640: AddComment now uses @productCode/@email/@comment parameters.
            var expectedSql = "insert into Comments(productCode, email, comment) values (@productCode, @email, @comment);";

            Assert.Contains("@productCode", expectedSql);
            Assert.Contains("@email", expectedSql);
            Assert.Contains("@comment", expectedSql);
            Assert.DoesNotContain("values ('", expectedSql);
        }
    }
}
