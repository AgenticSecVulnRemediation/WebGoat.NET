using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderEmailByCustomerNumberParameterizedTests
    {
        [Fact]
        public void GetEmailByCustomerNumber_UsesParameterizedExecuteScalar()
        {
            // Delta regression: string concatenation removed; ExecuteScalar now receives query with @customerNumber and MySqlParameter.
            var src = System.IO.File.ReadAllText("WebGoat/App_Code/DB/MySqlDbProvider.cs");

            Assert.Contains("SELECT email FROM CustomerLogin WHERE customerNumber = @customerNumber", src);
            Assert.Contains("new MySqlParameter(\"@customerNumber\"", src);
            Assert.DoesNotContain("select email from CustomerLogin where customerNumber = \" + num", src);
        }
    }
}
