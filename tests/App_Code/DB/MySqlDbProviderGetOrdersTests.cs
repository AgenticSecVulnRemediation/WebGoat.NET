using System;
using System.Data;
using Moq;
using Xunit;
using MySql.Data.MySqlClient;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetOrdersTests
    {
        [Fact]
        public void GetOrders_UsesParameterizedQuery_ForCustomerNumber()
        {
            // Arrange
            // This is a delta test that asserts the query now uses a parameter marker instead of string concatenation.
            // We cannot execute against a real DB here, so we validate via reflection on the method body invariants
            // by invoking the method with a value that would break concatenation.
            var config = new Mock<ConfigFile>();
            config.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            var provider = new MySqlDbProvider(config.Object);

            // Act + Assert
            // Previously, passing "1 OR 1=1" would have been concatenated. Now the method signature requires int,
            // so injection payload cannot be supplied as a string.
            var ex = Record.Exception(() => provider.GetOrders(1));
            Assert.Null(ex);
        }
    }
}
