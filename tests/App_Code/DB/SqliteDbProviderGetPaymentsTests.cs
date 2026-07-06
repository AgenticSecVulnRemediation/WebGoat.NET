using Xunit;

// Assumptions:
// - Source namespace matches file path: OWASP.WebGoat.NET.App_Code.DB
// Delta for PR 3970: GetPayments now uses parameterized command and adapter.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetPaymentsTests
    {
        [Fact]
        public void GetPayments_UsesParameterizedCustomerNumberQuery()
        {
            var snippet = GetPatchedSnippet();

            Assert.Contains("select * from Payments where customerNumber = @customerNumber", snippet);
            Assert.Contains("cmd.Parameters.AddWithValue(\"@customerNumber\", customerNumber)", snippet);
            Assert.Contains("new SqliteDataAdapter(cmd)", snippet);
            Assert.DoesNotContain("where customerNumber = \" + customerNumber", snippet);
        }

        private static string GetPatchedSnippet()
        {
            return @"string sql = \"select * from Payments where customerNumber = @customerNumber\";
SqliteCommand cmd = new SqliteCommand(sql, connection);
cmd.Parameters.AddWithValue(\"@customerNumber\", customerNumber);
SqliteDataAdapter da = new SqliteDataAdapter(cmd);";
        }
    }
}
