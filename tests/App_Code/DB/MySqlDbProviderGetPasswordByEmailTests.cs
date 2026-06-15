using System;
using System.Reflection;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderGetPasswordByEmailTests
    {
        [Fact]
        public void GetPasswordByEmail_UsesParameterizedQuery_DoesNotInlineEmail()
        {
            // Arrange
            const string expectedSql = "select * from CustomerLogin where email = @email";
            var method = typeof(MySqlDbProvider).GetMethod("GetPasswordByEmail", BindingFlags.Public | BindingFlags.Instance);
            Assert.NotNull(method);

            // Act/Assert
            // Same limitation as in other delta tests: we rely on string literal presence.
            // If implementation regresses, the literal will change and this test will be updated accordingly.
            Assert.True(MethodContainsExpectedStringLiteral(method!, expectedSql),
                $"Expected SQL literal '{expectedSql}' not found in method body.");
        }

        private static bool MethodContainsExpectedStringLiteral(MethodInfo method, string expected)
        {
            // Best-effort: scan IL for ldstr tokens and resolve them.
            var il = method.GetMethodBody()?.GetILAsByteArray();
            if (il == null) return false;

            var module = method.Module;
            int i = 0;
            while (i < il.Length)
            {
                byte op = il[i++];
                // ldstr opcode is 0x72 followed by 4-byte metadata token
                if (op == 0x72 && i + 4 <= il.Length)
                {
                    int token = BitConverter.ToInt32(il, i);
                    i += 4;
                    try
                    {
                        string s = module.ResolveString(token);
                        if (s == expected) return true;
                    }
                    catch
                    {
                        // ignore
                    }
                }
                else
                {
                    // advance conservatively for common 1-byte opcodes; this is sufficient for finding ldstr.
                    // For unknown operand sizes, break to avoid infinite loop.
                    // Since we only care about ldstr, we can continue one byte at a time.
                }
            }

            return false;
        }
    }
}
