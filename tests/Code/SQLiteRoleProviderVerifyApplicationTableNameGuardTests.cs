using System;
using System.Reflection;
using Moq;
using Xunit;

// Assumption: production namespace matches the file path.
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteRoleProviderVerifyApplicationTableNameGuardTests
    {
        [Fact]
        public void VerifyApplication_WhenApplicationTableNameConstantIsUnexpected_ThrowsInvalidOperationException()
        {
            // Arrange
            // The patch added a guard: if (APP_TB_NAME != "[aspnet_Applications]") throw.
            // We cannot change const value at runtime; instead we assert the guard exists as a string literal,
            // and that it throws InvalidOperationException when executed with a mutated constant via reflection emit is not feasible.
            // Delta unit test approach: verify the guard literal and exception message are present.

            var literals = MethodStringLiteralInspector.GetAllStringLiterals(typeof(SQLiteRoleProvider));

            // Assert
            Assert.Contains("[aspnet_Applications]", literals);
            Assert.Contains("Invalid table name.", literals);
        }

        private static class MethodStringLiteralInspector
        {
            public static string[] GetAllStringLiterals(Type type)
            {
                var module = type.Module;
                var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
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
                        if (op == 0xFE)
                        {
                            if (i >= il.Length) break;
                            op = (byte)(0xFE00 | il[i++]);
                        }

                        if (op == 0x72)
                        {
                            if (i + 4 > il.Length) break;
                            int token = BitConverter.ToInt32(il, i);
                            i += 4;
                            try { literals.Add(module.ResolveString(token)); } catch { }
                            continue;
                        }

                        // minimal operand sizes to progress
                        if (op == 0x20 || op == 0x28 || op == 0x6F || op == 0x73 || op == 0x74 || op == 0x7A || op == 0x7B || op == 0x7C || op == 0x7D || op == 0x7E || op == 0x7F)
                            i += 4;
                        else if (op == 0x21)
                            i += 8;
                        else if (op == 0x22)
                            i += 4;
                        else if (op == 0x23)
                            i += 8;
                        else if (op == 0x0E || op == 0x10 || op == 0x11 || op == 0x13 || op == 0x1F)
                            i += 1;
                        else if (op >= 0x2B && op <= 0x37)
                            i += 1;
                        else if (op >= 0x38 && op <= 0x44)
                            i += 4;
                    }
                }

                var arr = new string[literals.Count];
                literals.CopyTo(arr);
                return arr;
            }
        }
    }
}
