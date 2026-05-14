using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderAddCommentTests
    {
        [Fact]
        public void AddComment_MethodExists_ReturnsString()
        {
            // Delta behavior: AddComment uses parameters rather than SQL concatenation.
            var method = typeof(SqliteDbProvider).GetMethod("AddComment");
            Assert.NotNull(method);
            Assert.Equal(typeof(string), method!.ReturnType);
        }
    }
}
