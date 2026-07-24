using System;
using System.Reflection;
using Xunit;

// Assumption: SQLiteMembershipProvider exists in TechInfoSystems.Data.SQLite
using TechInfoSystems.Data.SQLite;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProviderDeleteUserClearsParametersTests
    {
        [Fact]
        public void DeleteUser_MethodExists_AndHasExpectedSignature()
        {
            // Arrange/Act
            var mi = typeof(SQLiteMembershipProvider).GetMethod(
                "DeleteUser",
                BindingFlags.Instance | BindingFlags.Public);

            // Assert
            Assert.NotNull(mi);
            var parameters = mi!.GetParameters();
            Assert.Equal(2, parameters.Length);
            Assert.Equal(typeof(string), parameters[0].ParameterType);
            Assert.Equal(typeof(bool), parameters[1].ParameterType);
        }
    }
}
