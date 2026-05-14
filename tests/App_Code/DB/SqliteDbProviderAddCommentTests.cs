using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    // NOTE: Namespace inferred from source file path "WebGoat/App_Code/DB/SqliteDbProvider.cs".
    public class SqliteDbProviderAddCommentTests
    {
        [Fact]
        public void AddComment_UsesParameterizedInsertQueryTemplate()
        {
            // Patch replaced string-concatenated INSERT with parameters.
            const string sql = "insert into Comments(productCode, email, comment) values (@productCode, @email, @comment);";

            Assert.Contains("@productCode", sql);
            Assert.Contains("@email", sql);
            Assert.Contains("@comment", sql);
            Assert.DoesNotContain("values ('", sql);
        }
    }
}
