using System;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProvider_GetPasswordByEmail_UsesParameterizedCommandTests
    {
        [Fact]
        public void GetPasswordByEmail_UsesEmailParameterInsteadOfConcatenation()
        {
            // Delta guard for PR #418: GetPasswordByEmail now uses a MySqlCommand with @Email parameter.
            var source = LoadSource();

            Assert.Contains("select * from CustomerLogin where email = @Email", source);
            Assert.Contains("new MySqlCommand(sql, connection)", source);
            Assert.Contains("Parameters.AddWithValue(\"@Email\"", source);
            Assert.Contains("new MySqlDataAdapter(cmd)", source);

            Assert.DoesNotContain("where email = '\" + email", source);
        }

        private static string LoadSource()
        {
            var path = System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "WebGoat", "App_Code", "DB", "MySqlDbProvider.cs");
            path = System.IO.Path.GetFullPath(path);
            return System.IO.File.ReadAllText(path);
        }
    }
}
