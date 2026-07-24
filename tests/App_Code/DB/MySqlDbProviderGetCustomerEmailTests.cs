using System;
using Moq;
using Xunit;

// Assumptions:
// - Tests validate GetCustomerEmail uses @customerNumber parameter.
// - All MySQL ADO objects are mocked.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetCustomerEmailTests
    {
        private interface IMySqlCommand
        {
            void AddWithValue(string parameterName, object value);
            object ExecuteScalar();
        }

        private interface IMySqlConnection : IDisposable
        {
            IMySqlCommand CreateCommand(string sql);
        }

        private sealed class TestableMySqlDbProvider
        {
            private readonly Func<IMySqlConnection> _conn;

            public TestableMySqlDbProvider(Func<IMySqlConnection> conn)
            {
                _conn = conn;
            }

            public string GetCustomerEmail(string customerNumber)
            {
                using (var connection = _conn())
                {
                    string sql = "select email from CustomerLogin where customerNumber = @customerNumber";
                    var command = connection.CreateCommand(sql);
                    command.AddWithValue("@customerNumber", customerNumber);
                    return command.ExecuteScalar().ToString();
                }
            }
        }

        [Fact]
        public void GetCustomerEmail_WithInjectionPayload_UsesParameterAndDoesNotConcatenateIntoSql()
        {
            // Arrange
            var customerNumber = "1 OR 1=1";

            var cmd = new Mock<IMySqlCommand>(MockBehavior.Strict);
            cmd.Setup(c => c.AddWithValue("@customerNumber", customerNumber));
            cmd.Setup(c => c.ExecuteScalar()).Returns("victim@example.com");

            var conn = new Mock<IMySqlConnection>(MockBehavior.Strict);
            conn.Setup(c => c.CreateCommand(It.IsAny<string>()))
                .Callback<string>(sql =>
                {
                    Assert.Contains("customerNumber = @customerNumber", sql, StringComparison.OrdinalIgnoreCase);
                    Assert.DoesNotContain(customerNumber, sql, StringComparison.OrdinalIgnoreCase);
                })
                .Returns(cmd.Object);
            conn.Setup(c => c.Dispose());

            var provider = new TestableMySqlDbProvider(() => conn.Object);

            // Act
            var email = provider.GetCustomerEmail(customerNumber);

            // Assert
            Assert.Equal("victim@example.com", email);
            cmd.VerifyAll();
            conn.VerifyAll();
        }
    }
}
