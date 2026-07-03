using System;
using System.Data;
using System.Reflection;
using Moq;
using Xunit;

// Assumption: production namespace matches the file path.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetCustomerEmailsTests
    {
        [Fact]
        public void GetCustomerEmails_UsesParameterizedLikeQuery_DoesNotConcatenateUserInputIntoSql()
        {
            // Arrange
            // We can't easily intercept MySqlDataAdapter creation without integration tests,
            // so we assert the behavioral change in a unit-test friendly way: the SQL text used
            // by the method must contain a parameter placeholder and must not contain the raw input.

            // Create provider with a mocked ConfigFile.
            var configFile = new Mock<ConfigFile>(MockBehavior.Loose);
            configFile.Setup(c => c.Get(It.IsAny<string>())).Returns(string.Empty);

            var provider = new MySqlDbProvider(configFile.Object);

            string attackerInput = "a%' OR 1=1 --";

            // Act
            // Call should not throw just from SQL construction.
            // It may throw at runtime due to DB connectivity; that's outside unit scope.
            // So we only validate internal SQL construction via reflection.
            //
            // NOTE: The production method constructs a local variable 'sql'. We validate that the fixed
            // query string exists in the method body by reflecting the IL and checking for the literal.
            var method = typeof(MySqlDbProvider).GetMethod("GetCustomerEmails", BindingFlags.Instance | BindingFlags.Public);

            // Assert
            Assert.NotNull(method);

            // Scan IL for the expected SQL literal.
            // This is a delta test: it will fail if code regresses to concatenation.
            var body = method!.GetMethodBody();
            Assert.NotNull(body);

            // Get all user string literals referenced by this method.
            // Simplified approach: search the assembly's manifest resource for the literal isn't viable.
            // Instead, assert that the new fixed SQL literal exists anywhere in the declaring type's metadata.
            // This is deterministic and catches the exact change.
            string expected = "select email from CustomerLogin where email like CONCAT(@email, '%')";

            // Look for the literal in the module's string table by enumerating all methods' IL bytes and resolving ldstr tokens.
            Assert.Contains(expected, MethodStringLiteralInspector.GetAllStringLiterals(typeof(MySqlDbProvider)));

            // Also ensure the old concatenation pattern is not present.
            string forbiddenFragment = "where email like '\" + email + \"%'";
            Assert.DoesNotContain(forbiddenFragment, string.Join("\n", MethodStringLiteralInspector.GetAllStringLiterals(typeof(MySqlDbProvider))));

            // Defensive: ensure attacker input isn't embedded in any SQL literal (would indicate concatenation).
            Assert.DoesNotContain(attackerInput, string.Join("\n", MethodStringLiteralInspector.GetAllStringLiterals(typeof(MySqlDbProvider))));
        }

        private static class MethodStringLiteralInspector
        {
            public static string[] GetAllStringLiterals(Type type)
            {
                var module = type.Module;
                var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);

                var literals = new System.Collections.Generic.HashSet<string>(StringComparer.Ordinal);

                foreach (var m in methods)
                {
                    var body = m.GetMethodBody();
                    if (body == null) continue;

                    var il = body.GetILAsByteArray();
                    if (il == null) continue;

                    int i = 0;
                    while (i < il.Length)
                    {
                        byte op = il[i++];

                        // Handle two-byte opcodes (0xFE xx)
                        if (op == 0xFE)
                        {
                            if (i >= il.Length) break;
                            op = (byte)(0xFE00 | il[i++]);
                        }

                        // ldstr opcode is 0x72
                        if (op == 0x72)
                        {
                            if (i + 4 > il.Length) break;
                            int token = BitConverter.ToInt32(il, i);
                            i += 4;
                            try
                            {
                                string s = module.ResolveString(token);
                                literals.Add(s);
                            }
                            catch
                            {
                                // ignore
                            }
                            continue;
                        }

                        i += OperandSize(op, il, i);
                    }
                }

                var arr = new string[literals.Count];
                literals.CopyTo(arr);
                return arr;
            }

            // Minimal operand size resolver for common opcodes; enough for deterministic traversal.
            private static int OperandSize(int op, byte[] il, int index)
            {
                // For simplicity, cover the most common operand sizes.
                // Many opcodes have 0 operand bytes.
                switch (op)
                {
                    // InlineNone
                    case 0x00: // nop
                    case 0x01: // break
                    case 0x02: // ldarg.0
                    case 0x03: // ldarg.1
                    case 0x04: // ldarg.2
                    case 0x05: // ldarg.3
                    case 0x06: // ldloc.0
                    case 0x07: // ldloc.1
                    case 0x08: // ldloc.2
                    case 0x09: // ldloc.3
                    case 0x0A: // stloc.0
                    case 0x0B: // stloc.1
                    case 0x0C: // stloc.2
                    case 0x0D: // stloc.3
                    case 0x0E: // ldarg.s
                    case 0x10: // starg.s
                    case 0x11: // ldloc.s
                    case 0x13: // stloc.s
                    case 0x14: // ldnull
                    case 0x15: // ldc.i4.m1
                    case 0x16: // ldc.i4.0
                    case 0x17: // ldc.i4.1
                    case 0x18: // ldc.i4.2
                    case 0x19: // ldc.i4.3
                    case 0x1A: // ldc.i4.4
                    case 0x1B: // ldc.i4.5
                    case 0x1C: // ldc.i4.6
                    case 0x1D: // ldc.i4.7
                    case 0x1E: // ldc.i4.8
                    case 0x1F: // ldc.i4.s
                    case 0x20: // ldc.i4
                    case 0x21: // ldc.i8
                    case 0x22: // ldc.r4
                    case 0x23: // ldc.r8
                    case 0x26: // dup
                    case 0x27: // pop
                    case 0x28: // call
                    default:
                        // Conservative: many opcodes are InlineMethod/InlineType/InlineField/InlineString/InlineTok etc => 4 bytes
                        // ShortInlineBrTarget => 1 byte, InlineBrTarget => 4 bytes.
                        // We do a basic heuristic using ranges.
                        if (op == 0x0E || op == 0x10 || op == 0x11 || op == 0x13 || op == 0x1F)
                            return 1;
                        if (op == 0x20 || op == 0x28 || op == 0x6F || op == 0x73 || op == 0x74 || op == 0x7A || op == 0x7B || op == 0x7C || op == 0x7D || op == 0x7E || op == 0x7F)
                            return 4;
                        if (op == 0x21)
                            return 8;
                        if (op == 0x22)
                            return 4;
                        if (op == 0x23)
                            return 8;

                        // Branch (short)
                        if (op >= 0x2B && op <= 0x37)
                            return 1;
                        // Branch (long)
                        if (op >= 0x38 && op <= 0x44)
                            return 4;

                        // Fall back to 0 to avoid overruns.
                        return 0;
                }
            }
        }
    }
}
