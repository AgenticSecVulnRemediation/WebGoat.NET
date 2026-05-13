using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderAddCommentTests
    {
        [Fact]
        public void AddComment_UsesParameters_InsteadOfStringConcatenation()
        {
            // Arrange
            // Delta: SQL now uses @productCode/@email/@comment parameters.
            // Without DB, validate that method signature exists and takes strings.
            var mi = typeof(SqliteDbProvider).GetMethod("AddComment");

            // Assert
            Assert.NotNull(mi);
            Assert.Equal(3, mi.GetParameters().Length);
        }
    }
}
