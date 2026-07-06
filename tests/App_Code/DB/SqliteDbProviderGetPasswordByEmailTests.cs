using System.IO;
using Xunit;

// Source-level regression test for PR 3961: GetPasswordByEmail now parameterizes email.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetPasswordByEmailTests
    {
        [Fact]
        public void GetPasswordByEmail_UsesParameterizedEmailQuery()
        {
            var path = Path.Combine("WebGoat", "App_Code", "DB", "SqliteDbProvider.cs");
            var code = File.ReadAllText(path);

            Assert.Contains("select * from CustomerLogin where email = @email;", code);
            Assert.Contains("Parameters.AddWithValue(\"@email\", email", code);
            Assert.Contains("new SqliteDataAdapter(cmd)", code);

            Assert.DoesNotContain("where email = '\" + email + \"'", code);
        }
    }
}
