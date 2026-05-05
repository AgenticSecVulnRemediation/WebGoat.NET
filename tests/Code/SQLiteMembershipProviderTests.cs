using System;
using System.Text.RegularExpressions;
using Xunit;

// Assumption: production namespace matches source file.
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderTests
    {
        [Fact]
        public void ChangePassword_RegexStrengthCheck_UsesTimeoutToPreventReDoS()
        {
            // Arrange
            // Security fix: Regex.IsMatch now uses a timeout overload to mitigate Regex DoS.
            // We assert the source contains the timeout overload call.
            var method = typeof(SQLiteMembershipProvider).GetMethod("ChangePassword");
            Assert.NotNull(method);

            // Act/Assert
            Assert.True(ContainsUserString(method!.Module, "TimeSpan.FromMilliseconds(500)"),
                "Expected regex match to include a timeout to mitigate ReDoS");
        }

        private static bool ContainsUserString(Module module, string expected)
        {
            try
            {
                var location = module.Assembly.Location;
                if (string.IsNullOrEmpty(location))
                    return false;

                var bytes = System.IO.File.ReadAllBytes(location);
                var text = System.Text.Encoding.UTF8.GetString(bytes);
                return text.Contains(expected, StringComparison.Ordinal);
            }
            catch
            {
                return false;
            }
        }
    }
}
