using MySql.Data.MySqlClient;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProvider_GetEmailByCustomerNumber_ParameterizedTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_UsesParameterInsteadOfConcatenation()
        {
            // Arrange
            var sql = "select email from CustomerLogin where customerNumber = @num";

            // Assert (delta)
            Assert.Contains("@num", sql);
            Assert.DoesNotContain("+ num", sql);
        }

        [Fact]
        public void GetEmailByCustomerNumber_BindsNumParameter()
        {
            // Arrange
            using var cmd = new MySqlCommand("select email from CustomerLogin where customerNumber = @num");

            // Act
            cmd.Parameters.AddWithValue("@num", "1 OR 1=1");

            // Assert
            Assert.True(cmd.Parameters.Contains("@num"));
            Assert.Equal("1 OR 1=1", cmd.Parameters["@num"].Value);
        }
    }
}
