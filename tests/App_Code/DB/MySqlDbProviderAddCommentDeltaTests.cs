// Assumptions:
// - Namespace is OWASP.WebGoat.NET.App_Code.DB
// - This delta test verifies AddComment was changed to use parameter placeholders and AddWithValue calls.
// - Deterministic verification by scanning compiled assembly strings.

using System.Reflection;
using Xunit;

namespace OWASP.WebGoat.NET.App_Code.DB.Tests
{
    public class MySqlDbProviderAddCommentDeltaTests
    {
        [Fact]
        public void AddComment_UsesParameterizedInsert()
        {
            var asm = typeof(OWASP.WebGoat.NET.App_Code.DB.MySqlDbProvider).Assembly;
            var allStrings = GetAllUserStrings(asm);

            Assert.Contains("INSERT INTO Comments(productCode, email, comment) VALUES (@productCode, @email, @comment)", allStrings);
            Assert.Contains("command.Parameters.AddWithValue(\"@productCode\"", allStrings);
            Assert.Contains("command.Parameters.AddWithValue(\"@email\"", allStrings);
            Assert.Contains("command.Parameters.AddWithValue(\"@comment\"", allStrings);
        }

        private static string GetAllUserStrings(Assembly asm)
        {
            var location = asm.Location;
            if (string.IsNullOrWhiteSpace(location) || !System.IO.File.Exists(location))
            {
                return string.Empty;
            }

            var bytes = System.IO.File.ReadAllBytes(location);
            return System.Text.Encoding.UTF8.GetString(bytes);
        }
    }
}
