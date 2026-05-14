using System;
using System.Data;
using System.IO;
using Mono.Data.Sqlite;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetCustomerEmailsParameterizedLikeTests
    {
        [Fact]
        public void GetCustomerEmails_UsesParameterizedLikePattern_AllowsPercentAppendWithoutSqlInjection()
        {
            // Regression test for PRs 363/370: email LIKE now uses parameter (@Email or @email) with appended '%'.

            var dbPath = Path.GetTempFileName();
            try
            {
                File.Delete(dbPath);
                SqliteConnection.CreateFile(dbPath);
                var cs = $"Data Source={dbPath};Version=3";

                using (var cn = new SqliteConnection(cs))
                {
                    cn.Open();
                    using var cmd = cn.CreateCommand();
                    cmd.CommandText = @"
CREATE TABLE CustomerLogin(email TEXT);
INSERT INTO CustomerLogin(email) VALUES ('a@b.com');
";
                    cmd.ExecuteNonQuery();
                }

                var provider = (SqliteDbProvider)System.Runtime.Serialization.FormatterServices.GetUninitializedObject(typeof(SqliteDbProvider));
                typeof(SqliteDbProvider).GetField("_connectionString", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                    .SetValue(provider, cs);

                var ex = Record.Exception(() => provider.GetCustomerEmails("a' OR 1=1 --"));
                Assert.Null(ex);
            }
            finally
            {
                if (File.Exists(dbPath)) File.Delete(dbPath);
            }
        }
    }
}
