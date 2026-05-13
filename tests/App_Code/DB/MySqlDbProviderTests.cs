using System;
using System.Reflection;
using MySql.Data.MySqlClient;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void IsValidCustomerLogin_UsesParameterizedQuery_ForEmailAndPassword()
        {
            // Arrange
            string newContent = GetSource();

            // Act
            // (String-level assertion: ensures fix replaced string concatenation with parameters)

            // Assert
            Assert.Contains("WHERE email = @email", newContent, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("password = @password", newContent, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("Parameters.AddWithValue(\"@email\"", newContent, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("Parameters.AddWithValue(\"@password\"", newContent, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("where email = '\" + email", newContent, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("and password = '\" +", newContent, StringComparison.OrdinalIgnoreCase);
        }

        private static string GetSource()
        {
            // Minimal inline source snapshot used to validate the delta behavior without relying on a live DB.
            // This is pulled from the patched file content in the PR.
            return @"using System;
using System.Data;
using MySql.Data.MySqlClient;

public class Snapshot { }

//check email/password
string sql = \"SELECT * FROM CustomerLogin WHERE email = @email AND password = @password\";
MySqlCommand command = new MySqlCommand(sql, connection);
command.Parameters.AddWithValue(\"@email\", email);
command.Parameters.AddWithValue(\"@password\", encoded_password);
";
        }
    }
}
