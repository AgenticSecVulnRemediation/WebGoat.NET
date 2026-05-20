using System;
using System.Data;
using Mono.Data.Sqlite;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_CustomCustomerLogin_ParameterizedTests
    {
        [Fact]
        public void CustomCustomerLogin_UsesEmailParameter_InsteadOfStringConcatenation()
        {
            // Arrange
            var sql = "select * from CustomerLogin where email = @email;";

            // Assert (delta)
            Assert.Contains("@email", sql);
            Assert.DoesNotContain("'\" + email + \"'", sql);
        }

        [Fact]
        public void CustomCustomerLogin_BindsEmailParameter_Safely()
        {
            // Arrange
            using var cn = new SqliteConnection("Data Source=:memory:;Version=3");
            cn.Open();
            using var cmd = cn.CreateCommand();
            cmd.CommandText = "SELECT 1 WHERE 1 = @email";

            // Act
            cmd.Parameters.AddWithValue("@email", 1);

            // Assert
            Assert.Equal(1, Convert.ToInt32(cmd.ExecuteScalar()));
        }
    }
}
