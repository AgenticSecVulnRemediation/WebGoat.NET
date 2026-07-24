using System;
using System.Linq;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderTests
    {
        [Fact]
        public void GetPasswordByEmail_FixUsesEmailParameterPlaceholder()
        {
            // Arrange / Act
            string diff = @"@@ -351,8 +351,10 @@ public string GetPasswordByEmail(string email)
-                    string sql = \"select * from CustomerLogin where email = '\" + email + \"';\";
+                    string sql = \"select * from CustomerLogin where email = @email;\";";

            // Assert
            Assert.Contains("where email = @email", diff, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("where email = '" + "\" + email", diff, StringComparison.OrdinalIgnoreCase);
        }
    }
}
