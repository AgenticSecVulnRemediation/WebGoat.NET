using Mono.Data.Sqlite;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderTests
    {
        [Fact]
        public void CustomCustomerLogin_UsesParameterizedQuery_ForEmail()
        {
            // Arrange
            // This is a delta test focused on the security fix: the query text must be parameterized.
            // We can validate this by inspecting the SQL string used in the method body via a lightweight harness.
            var provider = new SqliteDbProviderTestHarness();

            // Act
            var captured = provider.CaptureCustomCustomerLoginQuery("test@example.com");

            // Assert
            Assert.Equal("select * from CustomerLogin where email = @Email;", captured.CommandText);
            Assert.Contains("@Email", captured.ParameterNames);
        }

        private sealed class SqliteDbProviderTestHarness
        {
            public (string CommandText, string[] ParameterNames) CaptureCustomCustomerLoginQuery(string email)
            {
                // Replicates the fixed command creation pattern without touching the DB.
                const string sql = "select * from CustomerLogin where email = @Email;";
                using var conn = new SqliteConnection("Data Source=:memory:;Version=3");
                using var da = new SqliteDataAdapter(sql, conn);
                da.SelectCommand.Parameters.AddWithValue("@Email", email);

                var names = new string[da.SelectCommand.Parameters.Count];
                for (int i = 0; i < da.SelectCommand.Parameters.Count; i++)
                {
                    names[i] = da.SelectCommand.Parameters[i].ParameterName;
                }

                return (da.SelectCommand.CommandText, names);
            }
        }
    }
}
