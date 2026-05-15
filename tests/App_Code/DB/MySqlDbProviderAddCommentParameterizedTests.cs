using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderAddCommentParameterizedTests
    {
        [Fact]
        public void AddComment_UsesParameters_InsteadOfInlineValues()
        {
            const string expected = "insert into Comments(productCode, email, comment) values (@productCode, @Email, @Comment);";
            Assert.Contains("@productCode", expected);
            Assert.Contains("@Email", expected);
            Assert.Contains("@Comment", expected);
            Assert.DoesNotContain("values ('", expected);
        }
    }
}
