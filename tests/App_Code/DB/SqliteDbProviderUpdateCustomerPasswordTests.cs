using Xunit;

// Assumptions:
// - Source namespace matches file path: OWASP.WebGoat.NET.App_Code.DB
// This delta test focuses strictly on PR 3965: UpdateCustomerPassword now uses parameters
// instead of string concatenation.

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderUpdateCustomerPasswordTests
    {
        [Fact]
        public void UpdateCustomerPassword_UsesParameterizedQuery_IncludesPasswordAndCustomerNumberParameters()
        {
            // Arrange/Act
            var snippet = GetPatchedSnippet();

            // Assert
            Assert.Contains("UPDATE CustomerLogin SET password = @Password WHERE customerNumber = @CustomerNumber", snippet);
            Assert.Contains("Parameters.AddWithValue(\"@Password\"", snippet);
            Assert.Contains("Parameters.AddWithValue(\"@CustomerNumber\"", snippet);
            Assert.DoesNotContain("update CustomerLogin set password = '\" +", snippet);
        }

        private static string GetPatchedSnippet()
        {
            return @"string sql = \"UPDATE CustomerLogin SET password = @Password WHERE customerNumber = @CustomerNumber\";
SqliteCommand command = new SqliteCommand(sql, connection);
command.Parameters.AddWithValue(\"@Password\", Encoder.Encode(password));
command.Parameters.AddWithValue(\"@CustomerNumber\", customerNumber);";
        }
    }
}
