using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderCustomerEmailsParameterizedTests
    {
        [Fact]
        public void GetCustomerEmails_UsesParameterizedLike()
        {
            // Delta regression: LIKE clause now parameterized with @email and suffix %.
            var src = System.IO.File.ReadAllText("WebGoat/App_Code/DB/MySqlDbProvider.cs");

            Assert.Contains("where email like @email", src);
            Assert.Contains("AddWithValue(\"@email\"", src);
            Assert.DoesNotContain("where email like '\" + email + \"%'", src);
        }
    }
}
