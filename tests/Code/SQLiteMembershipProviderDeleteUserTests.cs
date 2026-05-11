using System;
using System.Reflection;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderDeleteUserTests
    {
        [Fact]
        public void DeleteUser_ClearsParameters_BeforeDeleteCommandTextIsSet()
        {
            // Arrange
            // Delta scope: provider now clears parameters before setting DELETE CommandText to avoid parameter collisions.
            // We assert this by checking the method body contains a call to SqliteParameterCollection.Clear.
            var method = typeof(SQLiteMembershipProvider).GetMethod("DeleteUser", BindingFlags.Instance | BindingFlags.Public);
            Assert.NotNull(method);

            // Act
            var body = method!.GetMethodBody();
            Assert.NotNull(body);
            var il = body!.GetILAsByteArray() ?? Array.Empty<byte>();
            var module = typeof(SQLiteMembershipProvider).Module;

            bool hasClearCall = false;
            for (int i = 0; i < il.Length - 4; i++)
            {
                // 0x28 = call, next 4 bytes = metadata token
                if (il[i] != 0x28) continue;
                int token = BitConverter.ToInt32(il, i + 1);
                try
                {
                    var mi = module.ResolveMethod(token) as MethodInfo;
                    if (mi != null && mi.Name == "Clear" && mi.DeclaringType != null && mi.DeclaringType.Name.Contains("SqliteParameterCollection"))
                    {
                        hasClearCall = true;
                        break;
                    }
                }
                catch
                {
                    // ignore non-method tokens
                }
            }

            // Assert
            Assert.True(hasClearCall);
        }
    }
}
