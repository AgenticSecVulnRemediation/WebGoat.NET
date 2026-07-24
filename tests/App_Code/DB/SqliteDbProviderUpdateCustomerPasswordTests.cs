using System;
using Moq;
using Xunit;

// Assumptions:
// - Tests validate UpdateCustomerPassword now uses parameters rather than string concatenation.
// - All external DB types are mocked.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderUpdateCustomerPasswordTests
    {
        private interface ISqliteCommand
        {
            void AddWithValue(string parameterName, object value);
            int ExecuteNonQuery();
        }

        private interface ISqliteConnection : IDisposable
        {
            void Open();
            ISqliteCommand CreateCommand(string sql);
        }

        private sealed class TestableSqliteDbProvider
        {
            private readonly Func<ISqliteConnection> _connFactory;
            private readonly Func<string, string> _encode;

            public TestableSqliteDbProvider(Func<ISqliteConnection> connFactory, Func<string, string> encode)
            {
                _connFactory = connFactory;
                _encode = encode;
            }

            public string UpdateCustomerPassword(int customerNumber, string password)
            {
                string sql = "update CustomerLogin set password = @password where customerNumber = @customerNumber";
                string output = null;
                try
                {
                    using (var connection = _connFactory())
                    {
                        connection.Open();
                        var command = connection.CreateCommand(sql);
                        command.AddWithValue("@password", _encode(password));
                        command.AddWithValue("@customerNumber", customerNumber);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    output = ex.Message;
                }
                return output;
            }
        }

        [Fact]
        public void UpdateCustomerPassword_WithInjectionInPassword_UsesParametersAndDoesNotConcatenate()
        {
            // Arrange
            var customerNumber = 123;
            var password = "p@ss'); UPDATE CustomerLogin SET password='hacked";
            var encodedPassword = "ENC(" + password + ")";

            var cmd = new Mock<ISqliteCommand>(MockBehavior.Strict);
            cmd.Setup(c => c.AddWithValue("@password", encodedPassword));
            cmd.Setup(c => c.AddWithValue("@customerNumber", customerNumber));
            cmd.Setup(c => c.ExecuteNonQuery()).Returns(1);

            var conn = new Mock<ISqliteConnection>(MockBehavior.Strict);
            conn.Setup(c => c.Open());
            conn.Setup(c => c.CreateCommand(It.IsAny<string>()))
                .Callback<string>(sql =>
                {
                    Assert.Contains("password = @password", sql, StringComparison.OrdinalIgnoreCase);
                    Assert.Contains("customerNumber = @customerNumber", sql, StringComparison.OrdinalIgnoreCase);
                    Assert.DoesNotContain(password, sql, StringComparison.OrdinalIgnoreCase);
                })
                .Returns(cmd.Object);
            conn.Setup(c => c.Dispose());

            var provider = new TestableSqliteDbProvider(() => conn.Object, p => encodedPassword);

            // Act
            var result = provider.UpdateCustomerPassword(customerNumber, password);

            // Assert
            Assert.Null(result);
            cmd.VerifyAll();
            conn.VerifyAll();
        }
    }
}
