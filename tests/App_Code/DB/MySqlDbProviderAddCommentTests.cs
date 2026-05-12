using System.Reflection;
using OWASP.WebGoat.NET.App_Code.DB;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderAddCommentTests
    {
        [Fact]
        public void AddComment_SqlString_IsParameterized()
        {
            // Arrange/Act
            string source = typeof(MySqlDbProvider).ToString();

            // Assert (delta): insert should use parameters instead of concatenation
            Assert.Contains("values (@productCode, @email, @comment)", source);
        }
    }
}
