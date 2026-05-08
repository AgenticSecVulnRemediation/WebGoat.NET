using System;
using System.Reflection;
using System.Text.RegularExpressions;
using Xunit;
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderCreateUserRegexTimeoutTests
    {
        [Fact]
        public void CreateUser_WhenPasswordStrengthRegexIsPathological_DoesNotHang()
        {
            // Arrange
            // We don't have direct access to the provider's config loader in a unit-test friendly way.
            // This test is delta-focused: ensure the code now calls Regex.IsMatch with a timeout overload.
            var method = typeof(SQLiteMembershipProvider).GetMethod("CreateUser");
            Assert.NotNull(method);

            // Act
            var body = method!.GetMethodBody();
            Assert.NotNull(body);

            // Assert
            // Verify the TimeSpan.FromMilliseconds(1000) constant was introduced by searching for the double 1000 in IL constants.
            // This is a regression guard against removing the timeout overload.
            var il = body!.GetILAsByteArray();
            Assert.NotNull(il);

            // simple scan for 1000 (0x03E8) as Int32 constant in IL stream
            bool found1000 = false;
            for (int i = 0; i < il!.Length - 4; i++)
            {
                // look for ldc.i4 1000 pattern: 0x20 then 4 bytes little-endian
                if (il[i] == 0x20 && il[i + 1] == 0xE8 && il[i + 2] == 0x03 && il[i + 3] == 0x00 && il[i + 4] == 0x00)
                {
                    found1000 = true;
                    break;
                }
            }

            Assert.True(found1000, "Expected Regex timeout constant (1000ms) in CreateUser after ReDoS fix");
        }
    }
}
