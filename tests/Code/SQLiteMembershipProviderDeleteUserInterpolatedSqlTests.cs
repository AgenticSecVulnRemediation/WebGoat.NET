using System;
using System.Reflection;
using Xunit;

// Assumption: production namespace is TechInfoSystems.Data.SQLite as in the source file.
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderDeleteUserInterpolationTests
    {
        [Fact]
        public void DeleteUser_DeleteAllRelatedDataTrue_UsesInterpolatedQueryForUserIdLookupAndDelete()
        {
            // PR changes SQL concatenation to string interpolation for USER_TB_NAME when selecting/deleting.
            // We assert the updated SQL patterns exist in assembly text.

            var asmBytes = System.IO.File.ReadAllBytes(typeof(SQLiteMembershipProvider).Assembly.Location);
            var asmText = System.Text.Encoding.UTF8.GetString(asmBytes);

            Assert.Contains("SELECT UserId FROM {USER_TB_NAME} WHERE LoweredUsername = $Username AND ApplicationId = $ApplicationId", asmText);
            Assert.Contains("DELETE FROM {USER_TB_NAME} WHERE LoweredUsername = $Username AND ApplicationId = $ApplicationId", asmText);
        }
    }
}
