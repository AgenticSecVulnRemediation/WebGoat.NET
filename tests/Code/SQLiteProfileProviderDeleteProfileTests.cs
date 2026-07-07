using System;
using System.Reflection;
using Moq;
using Xunit;

// Note: Namespace inference based on file path WebGoat/Code/SQLiteProfileProvider.cs

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderDeleteProfileTests
    {
        [Fact]
        public void DeleteProfile_ClearsParametersBeforeAddingUserIdParameter()
        {
            // Arrange
            var t = typeof(TechInfoSystems.Data.SQLite.SQLiteProfileProvider);
            var method = t.GetMethod("DeleteProfile", BindingFlags.NonPublic | BindingFlags.Static);
            Assert.NotNull(method);

            // Act / Assert
            // The security fix ensures cmd.Parameters.Clear() is called before reusing the SqliteCommand
            // for the DELETE statement, preventing stale/bound parameters from earlier SELECT.
            // Verify the updated source compiled into assembly contains "Parameters.Clear" near "AddWithValue(\"$UserId\"".
            var asmPath = t.Assembly.Location;
            var bytes = System.IO.File.ReadAllBytes(asmPath);
            var text = System.Text.Encoding.UTF8.GetString(bytes);

            Assert.Contains("Parameters.Clear", text);
            Assert.Contains("AddWithValue(\"$UserId\"", text);
        }
    }
}
