using System;
using System.Collections;
using System.Data;
using Moq;
using MySql.Data.MySqlClient;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetPasswordByEmailParameterizedTests
    {
        [Fact]
        public void GetPasswordByEmail_UsesParameterizedSelect_BindsEmailParameter()
        {
            // Arrange
            var provider = CreateProvider();
            const string email = "user@example.com";

            MySqlCommand? capturedCommand = null;

            using (MySqlDataAdapterCommandCtorShim.Scope(scope =>
            {
                scope.OnCommandCtor = (sql, conn) =>
                {
                    capturedCommand = new MySqlCommand(sql, conn);
                    return capturedCommand;
                };

                // ensure Fill adds required structure
                scope.OnFill = (adapter, ds) =>
                {
                    var table = new DataTable();
                    table.Columns.Add("Password", typeof(string));
                    var row = table.NewRow();
                    row["Password"] = "ENC";
                    table.Rows.Add(row);
                    ds.Tables.Add(table);
                    return 1;
                };

                // Act
                provider.GetPasswordByEmail(email);
            }))
            {
                // Assert
                Assert.NotNull(capturedCommand);
                Assert.Equal("select * from CustomerLogin where email = @email;", capturedCommand!.CommandText);
                var emailParam = (MySqlParameter)Assert.Single(capturedCommand.Parameters, p => p.ParameterName == "@email");
                Assert.Equal(email, (string)emailParam.Value);
            }
        }

        private static MySqlDbProvider CreateProvider()
        {
            var config = new Mock<ConfigFile>(MockBehavior.Loose);
            config.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            return new MySqlDbProvider(config.Object);
        }

        private static class MySqlDataAdapterCommandCtorShim
        {
            public sealed class Scope : IDisposable
            {
                public Func<string, MySqlConnection, MySqlCommand>? OnCommandCtor;
                public Func<MySqlDataAdapter, DataSet, int>? OnFill;

                private static readonly AsyncLocal<Scope?> Current = new();
                private readonly Scope? _previous;

                public Scope(Action<Scope> setup)
                {
                    _previous = Current.Value;
                    Current.Value = this;
                    setup(this);

                    Hooks.CommandCtor = (sql, conn) =>
                    {
                        var scope = Current.Value;
                        return scope?.OnCommandCtor?.Invoke(sql, conn) ?? new MySqlCommand(sql, conn);
                    };
                    Hooks.Fill = (adapter, ds) =>
                    {
                        var scope = Current.Value;
                        return scope?.OnFill?.Invoke(adapter, ds) ?? adapter.Fill(ds);
                    };
                }

                public void Dispose()
                {
                    Hooks.CommandCtor = null;
                    Hooks.Fill = null;
                    Current.Value = _previous;
                }
            }

            internal static class Hooks
            {
                public static Func<string, MySqlConnection, MySqlCommand>? CommandCtor;
                public static Func<MySqlDataAdapter, DataSet, int>? Fill;
            }
        }
    }
}
