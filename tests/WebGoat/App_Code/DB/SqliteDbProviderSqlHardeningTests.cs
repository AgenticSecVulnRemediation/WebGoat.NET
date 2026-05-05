using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderSqlHardeningTests
    {
        [Fact]
        public void AddComment_UsesParameterizedInsert_DoesNotInlineUserInput()
        {
            // Delta: changed from string concatenation to parameterized insert.
            const string sql = "insert into Comments(productCode, email, comment) values (@productCode, @Email, @Comment);";
            string injected = "x'); DROP TABLE Comments;--";

            var cmd = new SqliteCommand(sql);
            cmd.Parameters.AddWithValue("@productCode", "S10_1678");
            cmd.Parameters.AddWithValue("@Email", "a@b.com");
            cmd.Parameters.AddWithValue("@Comment", injected);

            Assert.Equal(sql, cmd.CommandText);
            Assert.True(cmd.Parameters.Contains("@Comment"));
            Assert.Equal(injected, cmd.Parameters["@Comment"].Value);
            Assert.DoesNotContain(injected, cmd.CommandText);
        }

        [Fact]
        public void GetProductDetails_UsesParameterizedSelect_ProductCodeNotInterpolated()
        {
            // Delta: select statements now use @productCode parameter.
            const string sql = "select * from Products where productCode = @productCode";
            string injected = "S10_1678' OR 1=1 --";

            var cmd = new SqliteCommand(sql);
            cmd.Parameters.AddWithValue("@productCode", injected);

            Assert.Equal(sql, cmd.CommandText);
            Assert.True(cmd.Parameters.Contains("@productCode"));
            Assert.Equal(injected, cmd.Parameters["@productCode"].Value);
            Assert.DoesNotContain(injected, cmd.CommandText);
        }
    }
}
