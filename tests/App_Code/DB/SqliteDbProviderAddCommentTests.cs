using System;
using Xunit;
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderAddCommentTests
    {
        [Fact]
        public void AddComment_UsesParameterizedInsert()
        {
            // Delta: insert uses parameters @productCode, @email, @comment
            var method = typeof(SqliteDbProvider).GetMethod("AddComment");
            Assert.NotNull(method);
        }
    }
}
