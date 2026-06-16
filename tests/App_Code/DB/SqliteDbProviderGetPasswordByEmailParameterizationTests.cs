using System;
using Xunit;

// Note: assumes SqliteDbProvider namespace as per source.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetPasswordByEmailParameterizationTests
    {
        [Fact]
        public void GetPasswordByEmail_UsesParameterForEmail()
        {
            // Arrange
            var method = typeof(SqliteDbProvider).GetMethod("GetPasswordByEmail");
            Assert.NotNull(method);

            // Assert
            Assert.Contains("where email = @email", System.IO.File.ReadAllText(method!.Module.FullyQualifiedName));
        }
    }
}
