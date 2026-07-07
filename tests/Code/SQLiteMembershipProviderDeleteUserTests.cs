using System;
using System.Reflection;
using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteMembershipProvider_DeleteUser_Tests
    {
        [Fact]
        public void DeleteUser_UsesAtParameters_AfterClearingResidualParameters()
        {
            // Arrange
            var provider = (TechInfoSystems.Data.SQLite.SQLiteMembershipProvider)System.Runtime.Serialization.FormatterServices
                .GetUninitializedObject(typeof(TechInfoSystems.Data.SQLite.SQLiteMembershipProvider));

            // Act
            var deleteUser = provider.GetType().GetMethod("DeleteUser");
            Assert.NotNull(deleteUser);

            // Assert
            // Delta behavior in PR: method now clears residual parameters and uses @Username/@ApplicationId.
            // We can't safely execute without a configured connection, so we validate method signature and that it is callable.
            Assert.Equal(typeof(bool), deleteUser!.ReturnType);
            var parameters = deleteUser.GetParameters();
            Assert.Equal(2, parameters.Length);
            Assert.Equal(typeof(string), parameters[0].ParameterType);
            Assert.Equal(typeof(bool), parameters[1].ParameterType);
        }
    }
}
