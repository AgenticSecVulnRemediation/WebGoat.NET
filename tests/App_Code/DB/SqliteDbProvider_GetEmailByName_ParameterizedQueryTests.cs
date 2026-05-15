using System;
using System.Data;
using System.Runtime.Serialization;
using Mono.Data.Sqlite;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

// Tests the delta: GetEmailByName now parameterizes LIKE clause.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_GetEmailByName_ParameterizedQueryTests
    {
        private static SqliteDbProvider CreateProvider(string connectionString)
        {
            var provider = (SqliteDbProvider)FormatterServices.GetUninitializedObject(typeof(SqliteDbProvider));
            var field = typeof(SqliteDbProvider).GetField("_connectionString", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            field!.SetValue(provider, connectionString);
            return provider;
        }

        [Fact]
        public void GetEmailByName_WithQuoteInName_DoesNotThrowSqlSyntaxError()
        {
            // Arrange: shared in-memory DB
            var cs = "Data Source=file:memdb2?mode=memory&cache=shared";
            using var keeper = new SqliteConnection(cs);
            keeper.Open();

            using (var cmd = keeper.CreateCommand())
            {
                cmd.CommandText = "CREATE TABLE Employees (firstName TEXT, lastName TEXT, email TEXT);";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "INSERT INTO Employees(firstName, lastName, email) VALUES ('O''Brien', 'Test', 'obrien@example.com');";
                cmd.ExecuteNonQuery();
            }

            var provider = CreateProvider(cs);

            // Act: this input would break the old concatenated SQL (unescaped single quote)
            var ex = Record.Exception(() => provider.GetEmailByName("O'Brien"));

            // Assert: no syntax exception, and we can still get a result set.
            Assert.Null(ex);
            var ds = provider.GetEmailByName("O'Brien");
            Assert.NotNull(ds);
            Assert.True(ds!.Tables[0].Rows.Count >= 1);
        }
    }
}
