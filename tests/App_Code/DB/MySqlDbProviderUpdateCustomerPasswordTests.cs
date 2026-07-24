using System;
using Moq;
using Xunit;

// Assumptions:
// - Tests validate UpdateCustomerPassword now uses @password and @customerNumber parameters.
// - MySQL ADO objects are mocked.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderUpdateCustomerPasswordTests
    {
        private interface IMySqlCommand
        {
            void AddWithValue(string parameterName, object value);
            int ExecuteNonQuery();
        }

        private interface IMySqlConnection : IDisposable
        {
            IMySqlCommand CreateCommand(string sql);
        }

        private sealed class TestableMySqlDbProvider
        {
            private readonly Func<IMySqlConnection> _conn;
            private readonly Func<string, string> _encode;

            public TestableMySqlDbProvider(Func<IMySqlConnection> conn, Func<string, string> encode)
            {
                _conn = conn;
                _encode = encode;
            }

            public string UpdateCustomerPassword(int customerNumber, string password)
            {
                string sql = "update CustomerLogin set password = @password where customerNumber = @customerNumber";
                using (var connection = _conn())
                {
                    var command = connection.CreateCommand(sql);
                    command.AddWithValue("@password", _encode(password));
                    command.AddWithValue("@customerNumber", customerNumber);
                    command.ExecuteNonQuery();
                    return null;
                }
            }
        }

        [Fact]
        public void UpdateCustomerPassword_WithInjectionInPassword_UsesParameters()
        {
            // Arrange
            var customerNumber = 42;
            var password = "pw'); DROP TABLE CustomerLogin; --";
            var encoded = "ENC(" + password + ")";

            var cmd = new Mock<IMySqlCommand>(MockBehavior.Strict);
            cmd.Setup(c => c.AddWithValue("@password", encoded));
            cmd.Setup(c => c.AddWithValue("@customerNumber", customerNumber));
            cmd.Setup(c => c.ExecuteNonQuery()).Returns(1);

            var conn = new Mock<IMySqlConnection>(MockBehavior.Strict);
            conn.Setup(c => c.CreateCommand(It.IsAny<string>()))
                .Callback<string>(sql =>
                {
                    Assert.Contains("password = @password", sql, StringComparison.OrdinalIgnoreCase);
                    Assert.Contains("customerNumber = @customerNumber", sql, StringComparison.OrdinalIgnoreCase);
                    Assert.DoesNotContain(password, sql, StringComparison.OrdinalIgnoreCase);
                })
                .Returns(cmd.Object);
            conn.Setup(c => c.Dispose());

            var provider = new TestableMySqlDbProvider(() => conn.Object, _ => encoded);

            // Act
            var result = provider.UpdateCustomerPassword(customerNumber, password);

            // Assert
            Assert.Null(result);
            cmd.VerifyAll();
            conn.VerifyAll();
        }
    }
}
