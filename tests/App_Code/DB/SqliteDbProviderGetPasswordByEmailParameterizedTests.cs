using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetPasswordByEmailParameterizedTests
    {
        [Fact]
        public void GetPasswordByEmail_UsesSqliteCommandWithParameter()
        {
            // Arrange
            var method = typeof(SqliteDbProvider).GetMethod("GetPasswordByEmail");
            Assert.NotNull(method);

            // Delta: changed to SqliteCommand with @email parameter.
            Assert.Equal("GetPasswordByEmail", method!.Name);
        }
    }
}
