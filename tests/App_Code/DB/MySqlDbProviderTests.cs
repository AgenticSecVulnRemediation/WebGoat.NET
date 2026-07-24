using System;
using System.Collections;
using System.Data;
using System.Reflection;
using Moq;
using MySql.Data.MySqlClient;
using Xunit;

// Assumption: production code namespaces follow folder structure.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void IsValidCustomerLogin_UsesParameterizedQuery_AddsEmailAndPasswordParameters()
        {
            // Arrange
            var provider = CreateProvider();

            const string email = "a@b.com";
            const string password = "secret";

            MySqlDataAdapter? capturedAdapter = null;

            // Shim MySqlDataAdapter(string, MySqlConnection) and capture SQL/SelectCommand
            using (MySqlShim.Scope(scope =>
            {
                scope.OnDataAdapterCtorWithSql = (sql, conn) =>
                {
                    capturedAdapter = new MySqlDataAdapter();
                    // MySqlDataAdapter.SelectCommand is settable
                    capturedAdapter.SelectCommand = new MySqlCommand(sql, conn);
                    return capturedAdapter;
                };

                // Prevent actual DB calls
                scope.OnFill = (adapter, ds) =>
                {
                    // Ensure table exists
                    ds.Tables.Add(new DataTable());
                    return 0;
                };

                // Act
                provider.IsValidCustomerLogin(email, password);
            }))
            {
                // Assert
                Assert.NotNull(capturedAdapter);
                Assert.NotNull(capturedAdapter!.SelectCommand);
                Assert.Equal("select * from CustomerLogin where email = @Email and password = @Password;", capturedAdapter.SelectCommand.CommandText);

                Assert.Contains(capturedAdapter.SelectCommand.Parameters, p => p.ParameterName == "@Email" && (string)p.Value == email);
                Assert.Contains(capturedAdapter.SelectCommand.Parameters, p => p.ParameterName == "@Password" && p.Value is string);

                // Ensure encoded password parameter is not the raw password
                var pwdParam = (MySqlParameter)Assert.Single(capturedAdapter.SelectCommand.Parameters, p => p.ParameterName == "@Password");
                Assert.NotEqual(password, (string)pwdParam.Value);
            }
        }

        private static MySqlDbProvider CreateProvider()
        {
            var config = new Mock<ConfigFile>(MockBehavior.Loose);
            config.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);
            return new MySqlDbProvider(config.Object);
        }

        /// <summary>
        /// Minimal shim layer to intercept MySqlDataAdapter construction and Fill without touching external systems.
        /// This works by using reflection to replace constructors is not possible in .NET; instead we expose seams
        /// through delegates and use wrapper methods in tests via IDisposable scope.
        ///
        /// Assumption: test runner includes no special profiling; thus we rely on MySqlDataAdapter being created
        /// inside our shim by using the same public API and capturing the state.
        ///
        /// If project later adds a proper abstraction for DB access, these shims can be removed.
        /// </summary>
        private static class MySqlShim
        {
            public sealed class Scope : IDisposable
            {
                private readonly Action<Scope> _setup;
                public Func<string, MySqlConnection, MySqlDataAdapter>? OnDataAdapterCtorWithSql;
                public Func<MySqlDataAdapter, DataSet, int>? OnFill;

                private static readonly AsyncLocal<Scope?> Current = new();
                private readonly Scope? _previous;

                public Scope(Action<Scope> setup)
                {
                    _setup = setup;
                    _previous = Current.Value;
                    Current.Value = this;
                    _setup(this);

                    // Install hooks
                    MySqlDataAdapterHooks.CtorWithSql = (sql, conn) =>
                    {
                        var scope = Current.Value;
                        return scope?.OnDataAdapterCtorWithSql?.Invoke(sql, conn) ?? new MySqlDataAdapter(sql, conn);
                    };
                    MySqlDataAdapterHooks.Fill = (adapter, ds) =>
                    {
                        var scope = Current.Value;
                        return scope?.OnFill?.Invoke(adapter, ds) ?? adapter.Fill(ds);
                    };
                }

                public void Dispose()
                {
                    // Uninstall hooks
                    MySqlDataAdapterHooks.CtorWithSql = null;
                    MySqlDataAdapterHooks.Fill = null;
                    Current.Value = _previous;
                }
            }

            internal static class MySqlDataAdapterHooks
            {
                public static Func<string, MySqlConnection, MySqlDataAdapter>? CtorWithSql;
                public static Func<MySqlDataAdapter, DataSet, int>? Fill;
            }
        }
    }
}
