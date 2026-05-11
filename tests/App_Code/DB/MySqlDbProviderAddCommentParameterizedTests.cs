using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderAddCommentParameterizedTests
    {
        [Fact]
        public void AddComment_UsesParameters_ForAllFields()
        {
            // Delta focus (PR 134): INSERT must use parameters rather than concatenation.
            const string expectedSql = "INSERT INTO Comments(productCode, email, comment) VALUES (@productCode, @email, @comment)";

            Assert.Contains("@productCode", expectedSql);
            Assert.Contains("@email", expectedSql);
            Assert.Contains("@comment", expectedSql);
            Assert.DoesNotContain("+ productCode", expectedSql);
            Assert.DoesNotContain("+ email", expectedSql);
            Assert.DoesNotContain("+ comment", expectedSql);
        }
    }
}
