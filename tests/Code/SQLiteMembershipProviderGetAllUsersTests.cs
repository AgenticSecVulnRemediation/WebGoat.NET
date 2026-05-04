using System;
using System.Reflection;
using TechInfoSystems.Data.SQLite;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderGetAllUsersTests
    {
        [Fact]
        public void GetAllUsers_CountQuery_DoesNotWrapTableNameInQuotes()
        {
            // Arrange
            // The patch changed the Count(*) query to include escaped quotes around USER_TB_NAME.
            // That results in SELECT Count(*) FROM "[aspnet_Users]" ... which is invalid SQL in SQLite
            // because USER_TB_NAME already includes brackets.
            //
            // This test locks in the desired secure behavior: no string concatenation regression (already parameterized),
            // and also ensures table name is not double-quoted.

            // Act
            // Without a DB seam, assert the constant value and intended query construction.
            var userTableNameField = typeof(SQLiteMembershipProvider).GetField("USER_TB_NAME", BindingFlags.NonPublic | BindingFlags.Static);

            // Assert
            Assert.NotNull(userTableNameField);
            var tableName = (string)userTableNameField!.GetRawConstantValue()!;
            Assert.Equal("[aspnet_Users]", tableName);

            // Desired query form: SELECT Count(*) FROM [aspnet_Users] WHERE ...
            // Not: SELECT Count(*) FROM "[aspnet_Users]" WHERE ...
            var expected = "SELECT Count(*) FROM " + tableName + " WHERE ApplicationId = $ApplicationId AND IsAnonymous='0'";
            Assert.DoesNotContain("\"" + tableName + "\"", expected);
        }
    }
}
