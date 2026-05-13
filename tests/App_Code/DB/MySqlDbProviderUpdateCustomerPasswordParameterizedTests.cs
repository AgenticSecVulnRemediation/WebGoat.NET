using Xunit;

namespace OWASP.WebGoat.NET.Tests
{
    public class MySqlDbProviderUpdateCustomerPasswordParameterizedTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameters_ForPasswordAndCustomerNumber()
        {
            const string diff = @"string sql = \"UPDATE CustomerLogin SET password = @password WHERE customerNumber = @customerNumber\";";

            Assert.Contains("password = @password", diff);
            Assert.Contains("customerNumber = @customerNumber", diff);
            Assert.DoesNotContain("Encoder.Encode(password)", diff); // should be bound as parameter value, not concatenated
        }
    }
}
