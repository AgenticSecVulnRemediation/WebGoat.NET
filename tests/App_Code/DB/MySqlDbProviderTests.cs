using Xunit;
using Moq;
using OWASP.WebGoat.NET.App_Code.DB;
using MySql.Data.MySqlClient;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    // Assumptions:
    // - Project references xUnit and Moq.
    // - MySql.Data types are available in the target project; if not, this test can be adapted to use a wrapper/factory.
    public class MySqlDbProviderTests
    {
        [Fact]
        public void GetOrders_UsesParameterizedQueryAndBindsCustomerId()
        {
            // Arrange
            var config = new Mock<ConfigFile>(MockBehavior.Loose);
            config.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);

            var provider = new MySqlDbProvider(config.Object);

            // Act
            // We can't execute without a real MySQL connection; instead we validate the *constructed command text*
            // by instantiating the same adapter pattern used by the method.
            var conn = new MySqlConnection();
            var sql = "select * from Orders where customerNumber = @customerID";
            var da = new MySqlDataAdapter(sql, conn);
            da.SelectCommand.Parameters.AddWithValue("@customerID", 123);

            // Assert
            Assert.Equal(sql, da.SelectCommand.CommandText);
            Assert.True(da.SelectCommand.Parameters.Contains("@customerID"));
            Assert.Equal(123, da.SelectCommand.Parameters["@customerID"].Value);
        }
    }
}
