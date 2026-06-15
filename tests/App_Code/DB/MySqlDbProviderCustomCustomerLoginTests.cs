using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderCustomCustomerLoginTests
    {
        [Fact]
        public void CustomCustomerLogin_UsesParameterizedEmailQuery()
        {
            // Regression test for SQL injection fix: email concatenation removed.
            var path = System.IO.Path.Combine("WebGoat", "App_Code", "DB", "MySqlDbProvider.cs");
            var content = System.IO.File.ReadAllText(path);

            Assert.Contains("select * from CustomerLogin where email = @email", content);
            Assert.Contains("Parameters.AddWithValue(\"@email\"", content);
            Assert.DoesNotContain("where email = '\" + email", content);
        }
    }
}
