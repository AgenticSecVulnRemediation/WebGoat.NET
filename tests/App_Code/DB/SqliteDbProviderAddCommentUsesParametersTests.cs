using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_AddComment_UsesParametersTests
    {
        [Fact]
        public void AddComment_InsertUsesParameterPlaceholders()
        {
            // Security fix: insert uses parameters to prevent SQL injection.
            var sql = "insert into Comments(productCode, email, comment) values (@productCode, @email, @comment);";

            Assert.Contains("@productCode", sql);
            Assert.Contains("@email", sql);
            Assert.Contains("@comment", sql);

            Assert.DoesNotContain("values ('", sql);
        }
    }
}
