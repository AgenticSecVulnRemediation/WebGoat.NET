using System;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderIsUserInRoleTests
    {
        [Fact]
        public void IsUserInRole_UsesAtParameters_DoesNotUseDollarParameters()
        {
            // Delta test for PR #3973: IsUserInRole query was rewritten to use @ parameters
            // (and avoid concatenation/injection patterns).

            var sourcePath = System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "WebGoat", "Code", "SQLiteRoleProvider.cs");
            var text = System.IO.File.ReadAllText(sourcePath);

            Assert.Contains("LoweredUsername = @Username", text);
            Assert.Contains("@MembershipApplicationId", text);
            Assert.Contains("@RoleName", text);
            Assert.Contains("@ApplicationId", text);

            Assert.DoesNotContain("LoweredUsername = $Username", text);
            Assert.DoesNotContain("$RoleName", text);
            Assert.DoesNotContain("$MembershipApplicationId", text);
            Assert.DoesNotContain("$ApplicationId\"", text);
        }
    }
}
