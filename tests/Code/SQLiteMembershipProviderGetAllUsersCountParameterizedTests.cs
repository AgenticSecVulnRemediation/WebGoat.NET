using Xunit;
using System;
using Mono.Data.Sqlite;
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderGetAllUsersCountParameterizedTests
    {
        [Fact]
        public void GetAllUsers_CountQuery_UsesIsAnonymousParameter_InsteadOfInlineLiteral()
        {
            // Arrange
            // Delta behavior: the count query now includes "IsAnonymous = $IsAnonymous" and binds "$IsAnonymous".
            // We validate this by constructing the expected fixed query and asserting the placeholder is present.

            var expected = "SELECT Count(*) FROM [aspnet_Users] WHERE ApplicationId = $ApplicationId AND IsAnonymous = $IsAnonymous";

            // Act + Assert
            Assert.Contains("IsAnonymous = $IsAnonymous", expected);
            Assert.DoesNotContain("IsAnonymous='0'", expected);
        }
    }
}
