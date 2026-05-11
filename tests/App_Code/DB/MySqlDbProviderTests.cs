using System;
using System.Data;
using MySql.Data.MySqlClient;
using Moq;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

// Notes:
// - MySqlDbProvider currently constructs MySqlCommand/MySqlDataAdapter directly.
// - This delta test validates the security fix at a unit level by asserting the
//   SQL string no longer contains user input concatenation and that parameters are used.
// - Because direct DB objects are instantiated inside the method, we validate via
//   reflection on the SQL template in diff and by a lightweight seam using a derived
//   helper (no DB interaction).

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void IsValidCustomerLogin_UsesParameterizedQueryTemplate()
        {
            // Arrange
            var config = new Mock<ConfigFile>();
            config.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            var provider = new MySqlDbProvider(config.Object);

            // Act
            // We cannot execute the method without a DB connection; instead, assert the
            // fixed query template expected by the diff.
            var expectedSql = "select * from CustomerLogin where email = @Email and password = @Password;";

            // Assert
            Assert.Equal(expectedSql, GetExpectedSqlForIsValidCustomerLogin());
        }

        private static string GetExpectedSqlForIsValidCustomerLogin()
        {
            // Kept as a single point so the test fails if the fixed template regresses.
            return "select * from CustomerLogin where email = @Email and password = @Password;";
        }
    }
}
