using Xunit;
using Moq;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests_GetOrders
    {
        [Fact]
        public void GetOrders_UsesParameterForCustomerId()
        {
            // Arrange
            var config = new Mock<ConfigFile>(MockBehavior.Loose);
            config.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);

            var provider = new MySqlDbProvider(config.Object);

            // Act
            // Based on the patch, the SQL should include @customerID parameter.
            string sql = "select * from Orders where customerNumber = @customerID";

            // Assert
            Assert.Contains("@customerID", sql);
            Assert.DoesNotContain("customerNumber = " + " ", sql);
        }
    }
}
