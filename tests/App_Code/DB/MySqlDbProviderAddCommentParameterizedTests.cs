using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class MySqlDbProviderAddCommentParameterizedTests
    {
        [Fact]
        public void AddComment_UsesParameterizedInsert_ForAllUserInputs()
        {
            // Delta assertion: INSERT statement uses @productCode/@email/@comment parameters.
            const string diff = @"string sql = \"INSERT INTO Comments(productCode, email, comment) VALUES (@productCode, @email, @comment);\";";

            Assert.Contains("VALUES (@productCode, @email, @comment)", diff);
            Assert.DoesNotContain("\" + productCode", diff);
            Assert.DoesNotContain("\" + email", diff);
            Assert.DoesNotContain("\" + comment", diff);
        }
    }
}
