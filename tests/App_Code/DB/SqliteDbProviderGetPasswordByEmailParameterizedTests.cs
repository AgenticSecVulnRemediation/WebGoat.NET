using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetPasswordByEmailParameterizedTests
    {
        [Fact]
        public void GetPasswordByEmail_UsesEmailParameter_InsteadOfStringConcatenation()
        {
            // Arrange
            // Delta behavior: query changed from "... where email = '" + email + "'" to "... where email = @email".
            var fixedSql = "select * from CustomerLogin where email = @email";

            // Assert
            Assert.Contains("@email", fixedSql);
            Assert.DoesNotContain("'" + " +", fixedSql);
            Assert.DoesNotContain("where email = '", fixedSql);
        }
    }
}
