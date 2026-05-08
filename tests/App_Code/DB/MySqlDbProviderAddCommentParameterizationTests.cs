using System;
using System.Text;
using Xunit;

using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderAddCommentParameterizationTests
    {
        [Fact]
        public void AddComment_UsesParameterizedInsert()
        {
            // Delta regression test: INSERT now uses @productCode, @email, @comment parameters.
            var asmText = Encoding.UTF8.GetString(System.IO.File.ReadAllBytes(typeof(MySqlDbProvider).Assembly.Location));

            Assert.Contains("INSERT INTO Comments", asmText);
            Assert.Contains("VALUES (@productCode, @email, @comment)", asmText);
            Assert.Contains("Parameters.AddWithValue(\"@productCode\"", asmText);
            Assert.Contains("Parameters.AddWithValue(\"@email\"", asmText);
            Assert.Contains("Parameters.AddWithValue(\"@comment\"", asmText);
        }
    }
}
