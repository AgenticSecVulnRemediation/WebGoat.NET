using System;
using System.Data;
using Mono.Data.Sqlite;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

// Assumptions:
// - Delta behavior: GetCustomerEmails uses a parameter @Email with value email + "%".

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetCustomerEmailsAdapterTests
    {
        [Fact]
        public void GetCustomerEmails_ParameterizesEmailLike_PreventsSqlInjection()
        {
            // Arrange
            var cs = "Data Source=:memory:;Version=3;New=True;";
            using var conn = new SqliteConnection(cs);
            conn.Open();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
CREATE TABLE CustomerLogin(email TEXT);
INSERT INTO CustomerLogin(email) VALUES ('x@x.com');
";
                cmd.ExecuteNonQuery();
            }

            var provider = (SqliteDbProvider)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(SqliteDbProvider));
            typeof(SqliteDbProvider).GetField("_connectionString", BindingFlags.NonPublic | BindingFlags.Instance)!
                .SetValue(provider, cs);

            // Act
            var ds = provider.GetCustomerEmails("x%' OR 1=1 --");

            // Assert
            // With parameterization, this should not match the row (prefix includes quotes/spaces).
            Assert.NotNull(ds);
            Assert.Equal(0, ds.Tables[0].Rows.Count);
        }
    }
}
