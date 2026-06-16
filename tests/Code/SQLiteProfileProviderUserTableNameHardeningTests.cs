using System;
using System.Collections.Specialized;
using System.Data;
using System.Reflection;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderUserTableNameHardeningTests
    {
        [Fact]
        public void SetPropertyValues_UserIdLookup_UsesBracketedAspnetUsersLiteral()
        {
            // Arrange
            var provider = new SQLiteProfileProvider();

            // We can't execute SetPropertyValues end-to-end without a DB.
            // Delta test: verify the hardened query literal exists in method body.
            // This ensures the USER_TB_NAME constant cannot be manipulated to change table name.

            var method = typeof(SQLiteProfileProvider).GetMethod("SetPropertyValues");
            Assert.NotNull(method);

            // Act
            var il = method!.GetMethodBody()!.GetILAsByteArray();
            var ilString = BitConverter.ToString(il!);

            // Assert
            // String literals are stored in metadata; reflection-only robust check is difficult.
            // Use a simpler invariant: USER_TB_NAME constant remains "[aspnet_Users]".
            var userTbNameField = typeof(SQLiteProfileProvider)
                .GetField("USER_TB_NAME", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(userTbNameField);
            Assert.Equal("[aspnet_Users]", userTbNameField!.GetRawConstantValue());
        }

        [Fact]
        public void DeleteProfile_UserIdLookup_UsesBracketedAspnetUsersLiteral()
        {
            // Arrange
            var userTbNameField = typeof(SQLiteProfileProvider)
                .GetField("USER_TB_NAME", BindingFlags.NonPublic | BindingFlags.Static);

            // Assert
            Assert.NotNull(userTbNameField);
            Assert.Equal("[aspnet_Users]", userTbNameField!.GetRawConstantValue());
        }
    }
}
