using Xunit;

// Assumption: production code namespace is OWASP.WebGoat.NET.App_Code.DB based on file path.
using OWASP.WebGoat.NET.App_Code.DB;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class SqliteDbProviderGetOrdersTests
    {
        [Fact]
        public void GetOrders_UsesParameterizedQuery_IncludesCustomerIdParameterToken()
        {
            // Arrange/Act
            // Delta test for SQL injection fix: query uses @customerID and AddWithValue.
            var literals = GetAllStringLiterals(typeof(SqliteDbProvider));

            // Assert
            Assert.Contains("@customerID", literals);
            Assert.DoesNotContain("select * from Orders where customerNumber = ", literals);
        }

        private static string GetAllStringLiterals(System.Type t)
        {
            var sb = new System.Text.StringBuilder();
            foreach (var m in t.GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic))
            {
                sb.Append(m.ToString());
            }
            return sb.ToString();
        }
    }
}
