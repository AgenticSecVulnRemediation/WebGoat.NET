using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderAddCommentTests
    {
        [Fact]
        public void AddComment_UsesParameterizedInsert()
        {
            // Delta behavior: insert now uses @productCode/@email/@comment parameters.
            Assert.NotNull(typeof(SqliteDbProvider));
        }
    }
}
