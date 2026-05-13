using System;
using System.Reflection;
using System.Text.RegularExpressions;
using Xunit;

// Assumption: production namespace is TechInfoSystems.Data.SQLite as declared in SQLiteMembershipProvider.cs.
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderRegexTimeoutTests
    {
        [Fact]
        public void ChangePassword_WithPasswordStrengthRegex_DoesNotHang_UsesRegexTimeout()
        {
            // Arrange
            // We directly test that the security fix is in place: Regex.IsMatch overload with timeout is used.
            // Since the provider pulls regex from config/internal static field, we set it via reflection.
            var providerType = typeof(SQLiteMembershipProvider);
            var regexField = providerType.GetField("_passwordStrengthRegularExpression", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(regexField);

            // A pathological regex that can cause catastrophic backtracking on long inputs.
            // (a+)+$ is a classic. With a timeout in the implementation, evaluation should be bounded.
            regexField!.SetValue(null, "^(a+)+$");

            var provider = new SQLiteMembershipProvider();

            // We cannot reliably execute ChangePassword end-to-end without DB/config.
            // Instead we validate the delta behavior via reflection on IL: the updated method must reference
            // Regex.IsMatch(string,string,RegexOptions,TimeSpan).
            var method = providerType.GetMethod("ChangePassword", BindingFlags.Public | BindingFlags.Instance);
            Assert.NotNull(method);

            // Act
            var ilBytes = method!.GetMethodBody()!.GetILAsByteArray();

            // Assert
            // The timeout overload is implemented by calling a method token that resolves to Regex.IsMatch with 4 params.
            // We conservatively assert that the method's metadata references System.TimeSpan (timeout parameter).
            // This avoids needing a full IL parser while still being a delta-focused regression guard.
            var module = providerType.Module;
            bool referencesTimeoutOverload = false;

            for (int i = 0; i < ilBytes.Length - 4; i++)
            {
                // Look for call/callvirt (0x28/0x6F) followed by 4-byte metadata token.
                byte op = ilBytes[i];
                if (op != 0x28 && op != 0x6F) continue;
                int token = BitConverter.ToInt32(ilBytes, i + 1);
                try
                {
                    var called = module.ResolveMethod(token);
                    if (called.DeclaringType == typeof(Regex) && called.Name == nameof(Regex.IsMatch))
                    {
                        var parameters = called.GetParameters();
                        if (parameters.Length == 4 && parameters[3].ParameterType == typeof(TimeSpan))
                        {
                            referencesTimeoutOverload = true;
                            break;
                        }
                    }
                }
                catch
                {
                    // ignore invalid tokens
                }
            }

            Assert.True(referencesTimeoutOverload,
                "ChangePassword should call Regex.IsMatch overload that includes a TimeSpan timeout to mitigate ReDoS.");
        }
    }
}
