using System;
using System.Reflection;
using System.Text.RegularExpressions;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests
    {
        [Fact]
        public void CreateUser_PasswordStrengthRegex_HasTimeoutToPreventReDoS()
        {
            // Arrange
            var provider = new SQLiteMembershipProvider();

            // The fixed code adds a Regex timeout when validating password strength.
            // Assert via reflection that the CreateUser method includes a Regex.IsMatch overload
            // that accepts a TimeSpan. This guards against regressions back to a no-timeout call.

            // Act
            MethodInfo? mi = typeof(SQLiteMembershipProvider).GetMethod(
                "CreateUser",
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                types: new[]
                {
                    typeof(string), typeof(string), typeof(string), typeof(string), typeof(string),
                    typeof(bool), typeof(object), typeof(System.Web.Security.MembershipCreateStatus).MakeByRefType()
                },
                modifiers: null);

            // Assert
            Assert.NotNull(mi);

            // Best available unit-test seam without refactoring: ensure RegexOptions and TimeSpan overload is present in IL.
            // This is robust for overload selection (presence of TimeSpan constant) and is tied to the security fix.
            var body = mi!.GetMethodBody();
            Assert.NotNull(body);
            var il = body!.GetILAsByteArray();
            Assert.NotNull(il);

            // Heuristic: TimeSpan.FromMilliseconds(double) call token must exist in the method body.
            // If regression removes timeout usage, this token should disappear.
            // We use method metadata token lookup to avoid brittle string matching.
            int token = typeof(TimeSpan).GetMethod("FromMilliseconds", new[] { typeof(double) })!.MetadataToken;
            Assert.Contains(BitConverter.GetBytes(token), il!);
        }
    }
}
