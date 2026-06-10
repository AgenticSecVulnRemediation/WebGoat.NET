using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Reflection;
using System.Web.Profile;
using Mono.Data.Sqlite;
using TechInfoSystems.Data.SQLite;
using Xunit;

// Assumptions:
// - The production project references Mono.Data.Sqlite and System.Web.Profile.
// - The provider uses private static fields for the connection string and membership application id.

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderUserIdLookupTests
    {
        [Fact]
        public void GetPropertyValuesFromDatabase_UsesAtParameters_ForUserIdLookup()
        {
            // Arrange
            // The vulnerability fix changed the UserId lookup in GetPropertyValuesFromDatabase from
            // "$UserName/$ApplicationId" placeholders to "@UserName/@ApplicationId".
            // This regression test ensures the compiled assembly now contains the safe placeholders.

            var method = typeof(SQLiteProfileProvider).GetMethod(
                "GetPropertyValuesFromDatabase",
                BindingFlags.NonPublic | BindingFlags.Static);

            Assert.NotNull(method);

            // Act
            var il = method!.GetMethodBody()!.GetILAsByteArray();
            var module = method.Module;

            // Extract all user strings referenced by the method via ldstr tokens.
            // This is a lightweight way to assert SQL template changes without requiring DB access.
            var strings = IlStringExtractor.ExtractStrings(module, il);

            // Assert
            Assert.Contains(strings, s => s.Contains("LoweredUsername = @UserName", StringComparison.Ordinal));
            Assert.Contains(strings, s => s.Contains("ApplicationId = @ApplicationId", StringComparison.Ordinal));

            // And ensure the old placeholders are not present in this query.
            Assert.DoesNotContain(strings, s => s.Contains("LoweredUsername = $UserName", StringComparison.Ordinal));
            Assert.DoesNotContain(strings, s => s.Contains("ApplicationId = $ApplicationId", StringComparison.Ordinal));
        }

        private static class IlStringExtractor
        {
            // Minimal IL parsing for ldstr (0x72) instructions.
            public static string[] ExtractStrings(Module module, byte[] il)
            {
                var list = new System.Collections.Generic.List<string>();

                int i = 0;
                while (i < il.Length)
                {
                    byte op = il[i++];

                    // ldstr
                    if (op == 0x72)
                    {
                        int token = BitConverter.ToInt32(il, i);
                        i += 4;

                        string s = module.ResolveString(token);
                        list.Add(s);
                        continue;
                    }

                    // Handle 2-byte opcodes (0xFE prefix)
                    if (op == 0xFE)
                    {
                        // Skip the next byte (extended opcode)
                        i++;
                        continue;
                    }

                    // Skip common operand sizes conservatively.
                    // For this targeted test we only need to reliably walk past ldstr.
                    // Many opcodes have no operand; for others, operands are 1/2/4/8 bytes.
                    // We'll use a minimal map for frequently encountered ones.
                    switch (op)
                    {
                        case 0x20: // ldc.i4
                        case 0x21: // ldc.i4.s (actually 1 byte) but safe to fallthrough with minimal handling
                        case 0x28: // call
                        case 0x6F: // callvirt
                        case 0x7B: // ldfld
                        case 0x7C: // ldflda
                        case 0x80: // stfld
                        case 0x8D: // newarr
                        case 0xA3: // stloc.s?
                            // Many of these are 4-byte metadata tokens.
                            i += 4;
                            break;
                        case 0x2A: // ret
                        case 0x00: // nop
                            break;
                        case 0x17: // ldc.i4.1
                        case 0x18: // ldc.i4.2
                        case 0x19: // ldc.i4.3
                        case 0x1A: // ldc.i4.4
                        case 0x1B: // ldc.i4.5
                        case 0x1C: // ldc.i4.6
                        case 0x1D: // ldc.i4.7
                        case 0x1E: // ldc.i4.8
                        case 0x1F: // ldc.i4.s (1 byte)
                        case 0x11: // ldloc.s
                        case 0x13: // stloc.s
                        case 0x0E: // ldarg.s
                        case 0x10: // starg.s
                            i += 1;
                            break;
                        default:
                            // Best-effort: do nothing; if we mis-step the test will fail and should be adjusted.
                            break;
                    }
                }

                return list.ToArray();
            }
        }
    }
}
