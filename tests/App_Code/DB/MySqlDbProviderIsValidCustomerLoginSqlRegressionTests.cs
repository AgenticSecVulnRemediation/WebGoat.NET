using System;
using System.IO;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderIsValidCustomerLoginSqlRegressionTests
    {
        private static string FindRepoRoot()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                if (Directory.Exists(Path.Combine(dir.FullName, "WebGoat")) && File.Exists(Path.Combine(dir.FullName, "WebGoat", "App_Code", "DB", "MySqlDbProvider.cs")))
                    return dir.FullName;
                dir = dir.Parent;
            }
            throw new DirectoryNotFoundException("Could not locate repo root containing 'WebGoat/App_Code/DB/MySqlDbProvider.cs'.");
        }

        [Fact]
        public void IsValidCustomerLogin_UsesParameterizedSqlAndDoesNotConcatenateUserInput()
        {
            // Delta assertion: query changed from string concatenation to parameterized SQL with @email/@password.
            var root = FindRepoRoot();
            var file = Path.Combine(root, "WebGoat", "App_Code", "DB", "MySqlDbProvider.cs");
            var text = File.ReadAllText(file);

            Assert.Contains("select * from CustomerLogin where email = @email and password = @password", text);
            Assert.Contains("cmd.Parameters.AddWithValue(\"@email\"", text);
            Assert.Contains("cmd.Parameters.AddWithValue(\"@password\"", text);

            // Ensure the previous vulnerable concatenation pattern is not present in IsValidCustomerLogin.
            Assert.DoesNotContain("where email = '\" + email", text);
            Assert.DoesNotContain("and password = '\" + encoded_password", text);
        }
    }
}
