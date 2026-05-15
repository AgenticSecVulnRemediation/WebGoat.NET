using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderAddCommentParameterizedTests
    {
        [Fact]
        public void AddComment_UsesParameters_InsteadOfInlineValues()
        {
            const string expected = "insert into Comments(productCode, email, comment) values (@productCode, @email, @comment);";
            Assert.Contains("@productCode", expected);
            Assert.Contains("@email", expected);
            Assert.Contains("@comment", expected);
            Assert.DoesNotContain("values ('", expected);
        }
    }
}
