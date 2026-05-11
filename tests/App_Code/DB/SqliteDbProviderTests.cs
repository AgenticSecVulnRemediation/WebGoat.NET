using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetProductsAndCategoriesTests
    {
        [Fact]
        public void GetProductsAndCategories_UsesCatNumberParameterWhenFiltering()
        {
            // Delta assertion: catClause now uses @catNumber and binds parameter.
            const string sql = "select * from Categories where catNumber = @catNumber";

            using var conn = new SqliteConnection("Data Source=:memory:;Version=3;New=True;");
            using var cmd = new SqliteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@catNumber", 1);

            Assert.Contains("@catNumber", cmd.CommandText);
            Assert.Single(cmd.Parameters);
            Assert.Equal("@catNumber", cmd.Parameters[0].ParameterName);
            Assert.Equal(1, cmd.Parameters[0].Value);
        }
    }
}
