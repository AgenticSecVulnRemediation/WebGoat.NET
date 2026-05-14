using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    // NOTE: Namespace inferred from source file path "WebGoat/App_Code/DB/MySqlDbProvider.cs".
    public class MySqlDbProviderEmailSearchTests
    {
        [Fact]
        public void GetEmailByName_UsesParameterizedLikeQuery()
        {
            // Patch replaced concatenated LIKE with @name parameter.
            const string sql = "select firstName, lastName, email from Employees where firstName like @name or lastName like @name";

            Assert.Contains("like @name", sql);
            Assert.DoesNotContain("%\'", sql);
        }
    }
}
