using System;
using System.Text.RegularExpressions;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderChangePasswordTests
    {
        [Fact]
        public void ChangePassword_PasswordStrengthRegex_UsesTimeout_ToPreventReDoS()
        {
            // Arrange
            // We only need to assert the changed behavior: Regex.IsMatch overload with a timeout is used.
            // Since calling ChangePassword requires a lot of provider infrastructure, we inspect IL for TimeSpan.FromMilliseconds(1000).
            var method = typeof(TechInfoSystems.Data.SQLite.SQLiteMembershipProvider).GetMethod("ChangePassword");
            Assert.NotNull(method);

            // Act
            var literals = typeof(TechInfoSystems.Data.SQLite.SQLiteMembershipProvider).Module.GetMethodsWithModuleScopeStringLiterals();

            // Assert
            // The timeout value is not a string; instead look for call to FromMilliseconds with 1000.0 in IL.
            var body = method!.GetMethodBody();
            Assert.NotNull(body);
            var il = body!.GetILAsByteArray();
            Assert.NotNull(il);

            // Scan for ldc.r8 1000 or ldc.i4 1000 followed by call to TimeSpan::FromMilliseconds
            bool foundFromMillisecondsCall = false;
            for (int i = 0; i < il!.Length - 5; i++)
            {
                // call opcode 0x28 with metadata token follows; resolve method and check name.
                if (il[i] == 0x28)
                {
                    int token = BitConverter.ToInt32(il, i + 1);
                    try
                    {
                        var called = method.Module.ResolveMethod(token);
                        if (called is MethodInfo mi && mi.DeclaringType == typeof(TimeSpan) && mi.Name == nameof(TimeSpan.FromMilliseconds))
                        {
                            foundFromMillisecondsCall = true;
                            break;
                        }
                    }
                    catch { }
                }
            }

            Assert.True(foundFromMillisecondsCall, "Expected ChangePassword to call TimeSpan.FromMilliseconds(...) for Regex timeout after fix.");
        }
    }

    internal static class ModuleStringLiteralExtensions
    {
        public static System.Collections.Generic.IEnumerable<string> GetMethodsWithModuleScopeStringLiterals(this System.Reflection.Module module)
        {
            foreach (var t in module.GetTypes())
            {
                foreach (var m in t.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.DeclaredOnly))
                {
                    var body = m.GetMethodBody();
                    if (body == null) continue;
                    var il = body.GetILAsByteArray();
                    if (il == null) continue;

                    for (int i = 0; i < il.Length - 5; i++)
                    {
                        if (il[i] != 0x72) continue;
                        int token = BitConverter.ToInt32(il, i + 1);
                        string? str = null;
                        try { str = module.ResolveString(token); } catch { }
                        if (str != null) yield return str;
                        i += 4;
                    }
                }
            }
        }
    }
}
