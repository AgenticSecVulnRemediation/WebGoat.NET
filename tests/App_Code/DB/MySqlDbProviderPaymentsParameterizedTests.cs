using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderPaymentsParameterizedTests
    {
        [Fact]
        public void GetPayments_UsesParameterizedCustomerNumber()
        {
            // Delta regression: concatenated customerNumber replaced with @customerNumber parameter.
            var src = System.IO.File.ReadAllText("WebGoat/App_Code/DB/MySqlDbProvider.cs");

            Assert.Contains("FROM Payments WHERE customerNumber = @customerNumber", src);
            Assert.Contains("Parameters.AddWithValue(\"@customerNumber\"", src);
            Assert.DoesNotContain("from Payments where customerNumber = \" + customerNumber", src);
        }
    }
}
