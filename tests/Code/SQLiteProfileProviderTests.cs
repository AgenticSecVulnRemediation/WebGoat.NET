using System;
using System.Data;
using Mono.Data.Sqlite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderTests
    {
        [Fact]
        public void GetPropertyValuesFromDatabase_UsesAtParameters_InsteadOfDollarParameters()
        {
            // Arrange
            // The fix changed only the parameter placeholders used when selecting UserId:
            // from $UserName/$ApplicationId to @UserName/@ApplicationId.
            // We assert that the updated source contains the safe placeholders.
            var source = typeof(SQLiteProfileProvider).Assembly
                .GetManifestResourceStream("TechInfoSystems.Data.SQLite.SQLiteProfileProvider.cs");

            // This project may not embed sources; in that case, fall back to verifying behavior via reflection.
            // Since runtime inspection of SqliteCommand text isn't possible without executing,
            // we instead validate the string literals via IL scan.
            // Act
            var method = typeof(SQLiteProfileProvider).GetMethod("GetPropertyValuesFromDatabase", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            Assert.NotNull(method);

            var body = method!.GetMethodBody();
            Assert.NotNull(body);

            var il = body!.GetILAsByteArray();
            Assert.NotNull(il);

            // Assert
            // Ensure new placeholders are present in the IL string table and old ones are not.
            var ilText = string.Join(" ", method!.Module.ResolveStringTokens(il!));
            Assert.Contains("LoweredUsername = @UserName", ilText);
            Assert.Contains("ApplicationId = @ApplicationId", ilText);
            Assert.DoesNotContain("LoweredUsername = $UserName", ilText);
            Assert.DoesNotContain("ApplicationId = $ApplicationId", ilText);
        }
    }

    internal static class ModuleStringTokenExtensions
    {
        public static string[] ResolveStringTokens(this System.Reflection.Module module, byte[] il)
        {
            var list = new System.Collections.Generic.List<string>();
            for (int i = 0; i < il.Length - 4; i++)
            {
                // ldstr opcode is 0x72 followed by 4-byte metadata token
                if (il[i] == 0x72)
                {
                    int token = BitConverter.ToInt32(il, i + 1);
                    try
                    {
                        list.Add(module.ResolveString(token));
                    }
                    catch
                    {
                        // ignore invalid tokens
                    }
                }
            }
            return list.ToArray();
        }
    }
}
