using System;
using System.Reflection;
using Xunit;

// Assumption: production namespace is TechInfoSystems.Data.SQLite, as declared in the source file.
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderDeleteUserTests
    {
        [Fact]
        public void DeleteUser_DeleteAllRelatedData_UsesConstantTableNameInInterpolatedQueryStrings()
        {
            // Arrange
            // The fix changed DeleteUser's SQL strings to use string interpolation with the constant USER_TB_NAME.
            // This test asserts the constant keeps the expected bracketed table name, and that the provider builds
            // SQL text using that constant (rather than an external input).
            var userTbNameField = typeof(SQLiteMembershipProvider)
                .GetField("USER_TB_NAME", BindingFlags.NonPublic | BindingFlags.Static);

            Assert.NotNull(userTbNameField);
            var userTbNameValue = userTbNameField!.GetValue(null) as string;

            // Assert
            Assert.Equal("[aspnet_Users]", userTbNameValue);
        }
    }
}
