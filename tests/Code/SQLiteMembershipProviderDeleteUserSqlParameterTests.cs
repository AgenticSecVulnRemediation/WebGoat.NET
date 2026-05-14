using System;
using System.Linq;
using System.Reflection;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderDeleteUserSqlParameterTests
    {
        [Fact]
        public void DeleteUser_UsesParameterizedDeleteQuery_DoesNotInlineUsername()
        {
            // This is a delta test derived from the security fix in PR #360.
            // The fix replaced $Username/$ApplicationId placeholders with @Username/@ApplicationId in the DELETE.
            // We assert that the provider's DeleteUser method now contains the secure parameter markers and
            // does not contain the old markers for the DELETE statement.

            var method = typeof(SQLiteMembershipProvider).GetMethod(
                "DeleteUser",
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                types: new[] { typeof(string), typeof(bool) },
                modifiers: null);

            Assert.NotNull(method);

            var il = method!.GetMethodBody()!.GetILAsByteArray();
            Assert.NotNull(il);

            // Heuristic: the method body should reference the new parameter tokens somewhere in its string constants.
            // We scan all user strings from metadata.
            var module = typeof(SQLiteMembershipProvider).Module;
            var strings = module.GetTypes()
                .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly))
                .Where(m => m.DeclaringType == typeof(SQLiteMembershipProvider) && m.Name == "DeleteUser")
                .SelectMany(m => ExtractUserStrings(m))
                .ToArray();

            Assert.Contains(strings, s => s.Contains("DELETE FROM") && s.Contains("@Username") && s.Contains("@ApplicationId"));
            Assert.DoesNotContain(strings, s => s.Contains("DELETE FROM") && s.Contains("$Username") && s.Contains("$ApplicationId"));
        }

        private static string[] ExtractUserStrings(MethodInfo method)
        {
            // Best-effort extraction of ldstr operands from method body.
            // This avoids requiring an actual SQLite database and focuses purely on the change in SQL parameter markers.
            var body = method.GetMethodBody();
            if (body == null) return Array.Empty<string>();

            var il = body.GetILAsByteArray();
            if (il == null) return Array.Empty<string>();

            var module = method.Module;
            var strings = new System.Collections.Generic.List<string>();

            int i = 0;
            while (i < il.Length)
            {
                OpCode code;
                byte b = il[i++];
                if (b == 0xFE)
                {
                    byte b2 = il[i++];
                    code = multiByteOpCodes[b2];
                }
                else
                {
                    code = singleByteOpCodes[b];
                }

                if (code == OpCodes.Ldstr)
                {
                    int token = BitConverter.ToInt32(il, i);
                    i += 4;
                    strings.Add(module.ResolveString(token));
                }
                else
                {
                    i += OperandSize(code.OperandType);
                }
            }

            return strings.ToArray();
        }

        private static int OperandSize(OperandType operandType)
        {
            return operandType switch
            {
                OperandType.InlineNone => 0,
                OperandType.ShortInlineBrTarget => 1,
                OperandType.ShortInlineI => 1,
                OperandType.ShortInlineVar => 1,
                OperandType.InlineVar => 2,
                OperandType.InlineI => 4,
                OperandType.InlineBrTarget => 4,
                OperandType.InlineField => 4,
                OperandType.InlineMethod => 4,
                OperandType.InlineSig => 4,
                OperandType.InlineString => 4,
                OperandType.InlineTok => 4,
                OperandType.InlineType => 4,
                OperandType.InlineSwitch => throw new NotSupportedException("InlineSwitch not supported in this lightweight scanner"),
                OperandType.InlineI8 => 8,
                OperandType.ShortInlineR => 4,
                OperandType.InlineR => 8,
                _ => 0
            };
        }

        private static readonly OpCode[] singleByteOpCodes = new OpCode[0x100];
        private static readonly OpCode[] multiByteOpCodes = new OpCode[0x100];

        static SQLiteMembershipProviderDeleteUserSqlParameterTests()
        {
            foreach (var fi in typeof(OpCodes).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                if (fi.GetValue(null) is OpCode op)
                {
                    ushort value = (ushort)op.Value;
                    if (value < 0x100)
                        singleByteOpCodes[value] = op;
                    else if ((value & 0xFF00) == 0xFE00)
                        multiByteOpCodes[value & 0xFF] = op;
                }
            }
        }
    }
}
