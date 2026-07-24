using System;
using System.Data;
using Moq;
using MySql.Data.MySqlClient;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderUpdateCustomerPasswordParameterizedTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameterizedUpdate_SetsPasswordAndCustomerNumberParameters()
        {
            // Arrange
            var provider = CreateProvider();

            const int customerNumber = 123;
            const string password = "pw";

            MySqlCommand? capturedCommand = null;

            using (MySqlCommandShim.Scope(scope =>
            {
                scope.OnCommandCtor = (sql, conn) =>
                {
                    capturedCommand = new MySqlCommand(sql, conn);
                    return capturedCommand;
                };

                scope.OnExecuteNonQuery = cmd => 1;

                // Act
                provider.UpdateCustomerPassword(customerNumber, password);
            }))
            {
                // Assert
                Assert.NotNull(capturedCommand);
                Assert.Equal("update CustomerLogin set password = @password where customerNumber = @customerNumber", capturedCommand!.CommandText);

                var pwd = (MySqlParameter)Assert.Single(capturedCommand.Parameters, p => p.ParameterName == "@password");
                var cust = (MySqlParameter)Assert.Single(capturedCommand.Parameters, p => p.ParameterName == "@customerNumber");

                Assert.NotEqual(password, (string)pwd.Value); // should be encoded
                Assert.Equal(customerNumber, (int)cust.Value);
            }
        }

        private static MySqlDbProvider CreateProvider()
        {
            var config = new Mock<ConfigFile>(MockBehavior.Loose);
            config.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            return new MySqlDbProvider(config.Object);
        }

        private static class MySqlCommandShim
        {
            public sealed class Scope : IDisposable
            {
                public Func<string, MySqlConnection, MySqlCommand>? OnCommandCtor;
                public Func<MySqlCommand, int>? OnExecuteNonQuery;

                private static readonly AsyncLocal<Scope?> Current = new();
                private readonly Scope? _previous;

                public Scope(Action<Scope> setup)
                {
                    _previous = Current.Value;
                    Current.Value = this;
                    setup(this);

                    MySqlCommandHooks.Ctor = (sql, conn) =>
                    {
                        var scope = Current.Value;
                        return scope?.OnCommandCtor?.Invoke(sql, conn) ?? new MySqlCommand(sql, conn);
                    };

                    MySqlCommandHooks.ExecuteNonQuery = cmd =>
                    {
                        var scope = Current.Value;
                        return scope?.OnExecuteNonQuery?.Invoke(cmd) ?? cmd.ExecuteNonQuery();
                    };
                }

                public void Dispose()
                {
                    MySqlCommandHooks.Ctor = null;
                    MySqlCommandHooks.ExecuteNonQuery = null;
                    Current.Value = _previous;
                }
            }

            internal static class MySqlCommandHooks
            {
                public static Func<string, MySqlConnection, MySqlCommand>? Ctor;
                public static Func<MySqlCommand, int>? ExecuteNonQuery;
            }
        }
    }
}
