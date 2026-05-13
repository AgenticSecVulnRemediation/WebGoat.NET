using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProvider_IsValidCustomerLogin_UsesParameterizedCommandTests
    {
        [Fact]
        public void IsValidCustomerLogin_UsesEmailAndPasswordParameters()
        {
            // Delta guard for PR #429: login query uses @email and @password parameters.
            var source = LoadSource();

            Assert.Contains("select * from CustomerLogin where email = @email and password = @password", source);
            Assert.Contains("Parameters.AddWithValue(\"@email\"", source);
            Assert.Contains("Parameters.AddWithValue(\"@password\"", source);
            Assert.Contains("new SqliteDataAdapter(command)", source);

            Assert.DoesNotContain("where email = '\" + email", source);
            Assert.DoesNotContain("password = '\" +", source);
        }

        private static string LoadSource()
        {
            var path = System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "WebGoat", "App_Code", "DB", "SqliteDbProvider.cs");
            path = System.IO.Path.GetFullPath(path);
            return System.IO.File.ReadAllText(path);
        }
    }
}
