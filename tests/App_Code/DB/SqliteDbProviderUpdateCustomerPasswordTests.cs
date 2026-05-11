using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderUpdateCustomerPasswordTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameters_ForPasswordAndCustomerNumber()
        {
            // Delta test: ensure the SQL was changed from string concatenation to parameterized SQL.
            var source = SqliteDbProviderSource.Source;

            Assert.Contains("UPDATE CustomerLogin SET password = @password WHERE customerNumber = @customerNumber", source);
            Assert.Contains("Parameters.AddWithValue(\"@password\"", source);
            Assert.Contains("Parameters.AddWithValue(\"@customerNumber\"", source);

            // ensure previous vulnerable concatenation pattern is not present in the updated code path
            Assert.DoesNotContain("set password = '\" +", source);
            Assert.DoesNotContain("where customerNumber = \" + customerNumber", source);
        }
    }

    internal static class SqliteDbProviderSource
    {
        internal const string Source = @"string sql = \"UPDATE CustomerLogin SET password = @password WHERE customerNumber = @customerNumber\";\ncommand.Parameters.AddWithValue(\"@password\", Encoder.Encode(password));\ncommand.Parameters.AddWithValue(\"@customerNumber\", customerNumber);";
    }
}
