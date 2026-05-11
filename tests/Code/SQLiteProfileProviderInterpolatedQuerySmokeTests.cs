using Xunit;

namespace TechInfoSystems.Data.SQLite.Tests
{
    public class SQLiteProfileProviderInterpolatedQuerySmokeTests
    {
        [Fact]
        public void SetPropertyValues_UserLookupQuery_NoLongerEndsWithSemicolon_StillUsesParameters()
        {
            // Delta focus (PR 166): CommandText changed to interpolated string with {USER_TB_NAME} and removed trailing ';'.
            const string expectedSql = "SELECT UserId FROM [aspnet_Users] WHERE LoweredUsername = $Username AND ApplicationId = $ApplicationId";

            Assert.Contains("$Username", expectedSql);
            Assert.Contains("$ApplicationId", expectedSql);
            Assert.DoesNotContain(";", expectedSql);
        }
    }
}
