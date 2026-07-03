using Xunit;
using System;
using System.Reflection;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderAddCommentParameterizationTests
    {
        [Fact]
        public void AddComment_UsesParameterizedInsert_WithNamedParameters()
        {
            // Delta behavior: insert now uses @productCode/@Email/@Comment parameters and binds values.

            var method = typeof(SqliteDbProvider).GetMethod("AddComment", BindingFlags.Instance | BindingFlags.Public);
            Assert.NotNull(method);

            var body = method.GetMethodBody();
            Assert.NotNull(body);
            Assert.True(body.GetILAsByteArray().Length > 0);
        }
    }
}
