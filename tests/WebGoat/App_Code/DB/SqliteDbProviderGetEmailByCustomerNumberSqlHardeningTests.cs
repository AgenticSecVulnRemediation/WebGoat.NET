using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetEmailByCustomerNumberSqlHardeningTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_UsesParameter_DoesNotInlineNum()
        {
            // Arrange
            var sql = "select email from CustomerLogin where customerNumber = @num";
            var num = "1 OR 1=1";

            // Act
            var cmd = new SqliteCommand();
            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("@num", num);

            // Assert
            Assert.Contains("@num", cmd.CommandText);
            Assert.DoesNotContain(num, cmd.CommandText);
            Assert.NotNull(cmd.Parameters["@num"]);
            Assert.Equal(num, cmd.Parameters["@num"].Value);
        }
    }
}
